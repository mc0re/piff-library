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
    public class PiffReader
    {
        #region Fields
        private static readonly Dictionary<string, Type> sBoxes;

        #endregion


        #region Init and clean-up

        static PiffReader()
        {
            var boxTypes =
                from t in Assembly.GetExecutingAssembly().GetTypes()
                let boxNameAttr = t.GetCustomAttribute<BoxNameAttribute>()
                where boxNameAttr != null
                group t by boxNameAttr.Name into g
                select new { Id = g.Key, Type = g.First() };

            sBoxes = boxTypes.ToDictionary(t => t.Id, t => t.Type);
        }

        #endregion


        #region Fragment API

        public static uint GetFragmentSequenceNumber(byte[] data)
        {
            var ms = new MemoryStream(data);
            var moof = ReadBox<PiffMovieFragment>(ms, new PiffReadContext());

            return moof.First<PiffMovieFragmentHeader>().Sequence;
        }


        public static uint GetTrackId(byte[] data)
        {
            var ms = new MemoryStream(data);
            var moof = ReadBox<PiffMovieFragment>(ms, new PiffReadContext());

            return moof.First<PiffTrackFragment>().First<PiffTrackFragmentHeader>().TrackId;
        }

        #endregion


        #region Box reading API

        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        internal static TBox ReadBox<TBox>(Stream input, PiffReadContext ctx) where TBox : PiffBoxBase
        {
            ReadBox(input, ctx, out var box);
            return box as TBox;
        }


        /// <summary>
        /// Read a box if it is of expected type. Back off if it's not.
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <returns>The number of bytes read or 0 if reached EOF</returns>
        internal static ulong ReadBox(Stream input, PiffReadContext ctx, out PiffBoxBase box)
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

            if (!sBoxes.TryGetValue(id, out var type))
            {
                ctx.AddWarning($"Unrecognized box '{id}' at position {startPosition}.");
                type = typeof(PiffCatchAllBox);
            }
            else if (! ctx.IsExpectedBoxType(type))
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
    }
}
