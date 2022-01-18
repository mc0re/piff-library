﻿using PiffLibrary.Boxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace PiffLibrary
{
    /// <summary>
    /// Reading data from the binary stream.
    /// </summary>
    public sealed class PiffReader
    {
        #region Constants

        /// <summary>
        /// For reading utility methods, this return code means "all ok for now".
        /// </summary>
        public const long Continue = 0;


        /// <summary>
        /// There was an unrecoverable error during reading.
        /// Skip to the end of the container box.
        /// </summary>
        public const long SkipToEnd = -1;


        /// <summary>
        /// End of file reached when trying to read a new box.
        /// File reading is finished successfully.
        /// </summary>
        public const long Eof = -2;


        /// <summary>
        /// End of file reached while expecting data.
        /// </summary>
        public const long EofPremature = -3;

        #endregion


        #region Fields

        /// <summary>
        /// Mapping between box name and type.
        /// The same type can have multiple names (see e.g. <see cref="PiffSkipBox"/>).
        /// </summary>
        private static readonly Dictionary<string, Type> sBoxNames = new Dictionary<string, Type>();


        /// <summary>
        /// Expected root-level box types.
        /// </summary>
        private static readonly Type[] sRootBoxes;


        /// <summary>
        /// Mapping between box type and expected children types.
        /// Unexpected types are reported as warnings.
        /// </summary>
        private static readonly Dictionary<Type, Type[]> sChildBoxes = new Dictionary<Type, Type[]>();

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Look for classes with <see cref="BoxNameAttribute"/> and process them.
        /// </summary>
        static PiffReader()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var boxNameAttrs = type.GetCustomAttributes<BoxNameAttribute>();
                if (!boxNameAttrs.Any()) continue;

                if (!typeof(PiffBoxBase).IsAssignableFrom(type))
                    throw new ArgumentException($"A box must inherit from {nameof(PiffBoxBase)}. {type.Name} does not.");

                foreach (var name in boxNameAttrs)
                {
                    // Fails when adding the same name twice
                    sBoxNames.Add(name.Name, type);
                }

                var children = type.GetCustomAttributes<ChildTypeAttribute>();
                if (children.Any())
                {
                    var childTypes = children.Select(t => t.ChildType).ToArray();

                    foreach (var child in childTypes)
                    {
                        if (!typeof(PiffBoxBase).IsAssignableFrom(child))
                            throw new ArgumentException($"A child box must inherit from {nameof(PiffBoxBase)}. {child.Name} does not.");
                    }

                    sChildBoxes.Add(type, childTypes);
                }
            }

            var rootTypes = typeof(PiffFile)
                .GetCustomAttributes<ChildTypeAttribute>()
                .Select(t => t.ChildType)
                .ToArray();

            foreach (var child in rootTypes)
            {
                if (!typeof(PiffBoxBase).IsAssignableFrom(child))
                    throw new ArgumentException($"A root box must inherit from {nameof(PiffBoxBase)}. {child.Name} does not.");
            }

            sRootBoxes = rootTypes;
        }

        #endregion


        #region Fragment API

        public static uint GetFragmentSequenceNumber(byte[] data)
        {
            using (var input = new BitReadStream(new MemoryStream(data), true))
            {
                var moof = ReadBox<PiffMovieFragmentBox>(input, new PiffReadContext { AnyRoot = true });

                return moof.First<PiffMovieFragmentHeaderBox>().Sequence;
            }
        }


        public static uint GetTrackId(byte[] data)
        {
            using (var input = new BitReadStream(new MemoryStream(data), true))
            {
                var moof = ReadBox<PiffMovieFragmentBox>(input, new PiffReadContext { AnyRoot = true });

                return moof.First<PiffTrackFragmentBox>().First<PiffTrackFragmentHeaderBox>().TrackId;
            }
        }

        #endregion


        #region Box reading API

        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <returns>
        /// The number of bytes read.
        /// - <see cref="Eof"/> if reached EOF at the expected place.
        /// - <see cref="EofPremature"/> if reached EOF at an unexpected place.
        /// - <see cref="SkipToEnd"/> if there was an error and we need to ignore
        ///   all data until the end of the container box.
        /// </returns>
        internal static long ReadBox(BitReadStream input, PiffReadContext ctx, out PiffBoxBase box)
        {
            var startPosition = input.Position;
            var header = (int) PiffBoxBase.HeaderLength;
            box = null;

            var length = ReadBoxLength(input, startPosition, ctx);
            if (length < 0) return length;

            var errorCode = ReadBoxName(input, startPosition, ctx, out var id);
            if (errorCode < 0) return length;

            if (length == PiffBoxBase.Length64)
            {
                // 64-bit size follows the box type
                length = ReadBoxLength64(input, startPosition, ctx, id);
                header += sizeof(ulong);
                if (length < 0) return length;
            }

            if (!sBoxNames.TryGetValue(id, out var type))
            {
                ctx.AddWarning($"Unrecognized box '{id}' at position {startPosition}.");
                type = typeof(PiffCatchAllBox);
            }
            else if (!ctx.AnyRoot && ! IsExpectedBoxType(ctx.CurrentBox?.GetType(), type))
            {
                ctx.AddWarning($"Unexpected child box '{id}' inside '{ctx.CurrentBoxName}' at position {startPosition}.");
            }

            box = (PiffBoxBase) Activator.CreateInstance(type);
            if (box is PiffCatchAllBox ca)
            {
                ca.BoxType = id;
            }

            ctx.Push(box, startPosition);
            var readBytes = PiffPropertyInfo.ReadObject(box, input, length - header, ctx);
            ctx.Pop();

            if (readBytes == SkipToEnd) return length;
            if (readBytes < 0) return readBytes;

            if (readBytes + header != length)
            {
                ctx.AddWarning($"Surplus {length - (readBytes + header)} bytes at the end of box '{id}' at position {startPosition}. Skipping.");
            }

            return length;
        }

        #endregion


        #region Utility

        /// <summary>
        /// Read a box of the expected type.
        /// </summary>
        private static TBox ReadBox<TBox>(BitReadStream input, PiffReadContext ctx) where TBox : PiffBoxBase
        {
            ReadBox(input, ctx, out var box);
            return box as TBox;
        }


        private static long ReadBoxLength(BitReadStream input, long startPosition, PiffReadContext ctx)
        {
            // In case it's 64-bit length, prepare a larger buffer
            var buf = new byte[sizeof(uint)];
            var bytesInLen = input.Read(buf, 0, sizeof(uint));

            // End of file
            if (bytesInLen == 0)
            {
                return Eof;
            }
            else if (bytesInLen < sizeof(uint))
            {
                ctx.AddError($"Malformed file: end of file when reading the next box length.");
                return EofPremature;
            }

            long length = buf.GetUInt32(0);

            if (length == 0)
            {
                // Box extends to the end of the file
                ctx.AddError($"Auto-extended boxes (at position {startPosition}) not implemented. Aborting.");
                return SkipToEnd;
            }
            else if (length != PiffBoxBase.Length64 && length < PiffBoxBase.HeaderLength)
            {
                ctx.AddWarning($"Too small box length {length} at position {startPosition}. Ignore.");
                return SkipToEnd;
            }

            return length;
        }


        private static long ReadBoxName(BitReadStream input, long startPosition, PiffReadContext ctx, out string boxId)
        {
            boxId = input.ReadAsciiString(PiffBoxBase.BoxTypeLength);

            if (boxId.Length != PiffBoxBase.BoxTypeLength)
            {
                ctx.AddError($"Malformed file: reached EOF when reading box name at position {startPosition}. Aborting.");
                return EofPremature;
            }

            if (!boxId.All(c => char.IsLetterOrDigit(c) || c == ' '))
            {
                ctx.AddError($"Malformed box name at position {startPosition}. Skipping.");
                return SkipToEnd;
            }

            return Continue;
        }


        private static long ReadBoxLength64(BitReadStream input, long startPosition, PiffReadContext ctx, string boxId)
        {
            var buf = new byte[sizeof(ulong)];
            var bytesInLen64 = input.Read(buf, 0, sizeof(ulong));

            if (bytesInLen64 < sizeof(ulong))
            {
                ctx.AddError($"Unexpected EOF inside box '{boxId}' header at position {startPosition}.");
                return EofPremature;
            }

            // We can't really support unsigned long, as length is reused for error reporting
            return buf.GetInt64(0);
        }


        private static bool IsExpectedBoxType(Type parentType, Type childType)
        {
            if (parentType is null)
                return sRootBoxes.Contains(childType);
            else
                return sChildBoxes[parentType].Contains(childType);
        }

        #endregion
    }
}
