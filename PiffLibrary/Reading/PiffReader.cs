using PiffLibrary.Boxes;
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

        static PiffReader()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var boxNameAttrs = type.GetCustomAttributes<BoxNameAttribute>();
                if (!boxNameAttrs.Any()) continue;

                foreach (var name in boxNameAttrs)
                {
                    sBoxNames.Add(name.Name, type);
                }

                var children = type.GetCustomAttributes<ChildTypeAttribute>();
                if (children.Any())
                {
                    sChildBoxes.Add(type, children.Select(t => t.Child).ToArray());
                }
            }

            sRootBoxes = typeof(PiffFile)
                .GetCustomAttributes<ChildTypeAttribute>()
                .Select(t => t.Child)
                .ToArray();
        }

        #endregion


        #region Fragment API

        public static uint GetFragmentSequenceNumber(byte[] data)
        {
            using (var input = new BitStream(new MemoryStream(data), true))
            {
                var moof = ReadBox<PiffMovieFragmentBox>(input, new PiffReadContext());

                return moof.First<PiffMovieFragmentHeaderBox>().Sequence;
            }
        }


        public static uint GetTrackId(byte[] data)
        {
            using (var input = new BitStream(new MemoryStream(data), true))
            {
                var moof = ReadBox<PiffMovieFragmentBox>(input, new PiffReadContext());

                return moof.First<PiffTrackFragmentBox>().First<PiffTrackFragmentHeaderBox>().TrackId;
            }
        }

        #endregion


        #region Box reading API

        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        internal static TBox ReadBox<TBox>(BitStream input, PiffReadContext ctx) where TBox : PiffBoxBase
        {
            ReadBox(input, ctx, out var box);
            return box as TBox;
        }


        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <returns>The number of bytes read or 0 if reached EOF</returns>
        internal static ulong ReadBox(BitStream input, PiffReadContext ctx, out PiffBoxBase box)
        {
            var startPosition = input.Position;
            var header = PiffBoxBase.HeaderLength;
            box = null;

            // In case it's 64-bit length, prepare a larger buffer
            var buf = new byte[sizeof(ulong)];
            var bytesInLen = input.Read(buf, 0, sizeof(uint));

            // End of file
            if (bytesInLen == 0) return 0;

            ulong length = buf.GetUInt32(0);
            
            if (length == 0)
            {
                // Box extends to the end of the file
                ctx.AddError($"Auto-extended boxes (at position {startPosition}) not implemented. Aborting.");
                return 0;
            }
            else if (length != PiffBoxBase.Length64 && length < PiffBoxBase.HeaderLength)
            {
                ctx.AddWarning($"Too small box length {length} at position {startPosition}. Ignore.");
                return length;
            }

            var id = input.ReadAsciiString(PiffBoxBase.BoxTypeLength);
            
            if (length == PiffBoxBase.Length64)
            {
                // 64-bit size follows the box type
                var bytesInLen64 = input.Read(buf, 0, sizeof(ulong));

                if (bytesInLen64 < sizeof(ulong))
                {
                    // Premature end of file
                    ctx.AddWarning($"Unexpected EOF inside box '{id}' header at position {startPosition}.");
                    return 0;
                }

                length = buf.GetUInt64(0);
                header += sizeof(ulong);
            }

            if (!sBoxNames.TryGetValue(id, out var type))
            {
                ctx.AddWarning($"Unrecognized box '{id}' at position {startPosition}.");
                type = typeof(PiffCatchAllBox);
            }
            else if (! IsExpectedBoxType(ctx.CurrentBox?.GetType(), type))
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

            return readBytes + header;
        }

        #endregion


        #region Utility

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
