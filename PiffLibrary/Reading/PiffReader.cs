using PiffLibrary.Boxes;
using PiffLibrary.Infrastructure;
using System;
using System.IO;
using System.Linq;


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
        private static readonly BoxStorage sBoxes = new BoxStorage();

        private static readonly byte[] LengthArray = new byte[sizeof(uint)];

        private static readonly byte[] Length64Array = new byte[sizeof(ulong)];


        #endregion


        #region Init and clean-up

        /// <summary>
        /// Look for classes with <see cref="BoxNameAttribute"/> and process them.
        /// </summary>
        static PiffReader()
        {
            sBoxes.Collect();
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
        internal static PiffReadStatuses ReadBox(BitReadStream input, PiffReadContext ctx, out PiffBoxBase box)
        {
            var startPosition = input.Position; // Used only for logs and messages
            var header = PiffBoxBase.HeaderLength;
            box = null;

            var statusLen = ReadBoxLength(input, ctx, startPosition, out var length);
            if (statusLen != PiffReadStatuses.Continue) return statusLen;

            var statusId = ReadBoxName(input, ctx, startPosition, out var id);
            if (statusId != PiffReadStatuses.Continue) return statusId;

            if (length == PiffBoxBase.Length64)
            {
                // 64-bit size follows the box type
                var statusLen64 = ReadBoxLength64(input, ctx, startPosition, id, out length);
                if (statusLen64 != PiffReadStatuses.Continue) return statusLen64;
                
                header += sizeof(ulong);
                ctx.AddInfo($"Box '{id}' at position {startPosition} has 64-bit length {length}.");
            }

            switch (sBoxes.FindBox(ctx.CurrentBox?.GetType(), id, out var type))
            {
                case FindBoxResults.Unrecognized:
                    ctx.AddWarning($"Unrecognized box '{id}' at position {startPosition}.");
                    break;

                case FindBoxResults.Ambiguous:
                    ctx.AddError($"More than one child of {ctx.CurrentBox?.GetType().Name} has name '{id}' at position {startPosition}.");
                    break;

                case FindBoxResults.Unexpected when !ctx.AnyRoot:
                    ctx.AddWarning($"Unexpected child box '{id}' inside '{ctx.CurrentBoxName}' at position {startPosition}.");
                    break;
            }

            box = (PiffBoxBase) Activator.CreateInstance(type);
            box.BoxType = id;

            var bodyLength = length - header;
            
            if (bodyLength > 0)
            {
                if (bodyLength > input.BytesLeft)
                {
                    ctx.AddError($"Box length claims {bodyLength} bytes, only {input.BytesLeft} bytes left in box '{id}' at position {startPosition}. Adjusting.");
                    bodyLength = input.BytesLeft;
                }

                using (var inputSlice = new BitReadStream(input, bodyLength, id))
                {
                    ctx.Push(box, startPosition);
                    var statusProps = PiffPropertyInfo.ReadObject(box, inputSlice, ctx);
                    ctx.Pop();

                    var bytesLeft = inputSlice.BytesLeft;
                    input.Advance(bodyLength - bytesLeft);

                    if (bytesLeft > 0)
                    {
                        ctx.AddWarning($"Extra {bytesLeft} bytes at the end of box '{id}' at position {startPosition}. Skipping.");
                        input.Seek(bytesLeft);
                    }

                    return statusProps;
                }
            }

            return PiffReadStatuses.Continue;
        }

        #endregion


        #region Utility

        /// <summary>
        /// Read a box of the expected type.
        /// </summary>
        /// <returns>Box or <see langword="null"/></returns>
        private static TBox ReadBox<TBox>(BitReadStream input, PiffReadContext ctx) where TBox : PiffBoxBase
        {
            if (ReadBox(input, ctx, out var box) != PiffReadStatuses.Continue)
                return null;

            return box as TBox;
        }


        /// <summary>
        /// Read the 4-byte box length.
        /// </summary>
        private static PiffReadStatuses ReadBoxLength(BitReadStream input, PiffReadContext ctx, long startPosition, out ulong length)
        {
            // In case it's 64-bit length, prepare a larger buffer
            var bytesInLen = input.Read(LengthArray, 0, sizeof(uint));

            // End of file
            if (bytesInLen < sizeof(uint))
            {
                length = 0;

                if (bytesInLen == 0)
                {
                    return PiffReadStatuses.Eof;
                }
                else
                {
                    ctx.AddError($"Malformed file: end of file when reading the next box length.");
                    return PiffReadStatuses.EofPremature;
                }
            }

            length = LengthArray.GetUInt32(0);

            if (length == PiffBoxBase.AutoExtend)
            {
                ctx.AddInfo($"Auto-extended box found at position {startPosition}.");
                length = ulong.MaxValue;
            }
            else if (length == PiffBoxBase.Length64)
            {
            }
            else if (length < PiffBoxBase.HeaderLength)
            {
                ctx.AddWarning($"Incorrect box length {length} at position {startPosition}. Start reading anew.");
                return PiffReadStatuses.SkipToEnd;
            }

            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read box name.
        /// </summary>
        private static PiffReadStatuses ReadBoxName(BitReadStream input, PiffReadContext ctx, long startPosition, out string boxId)
        {
            var status = input.ReadAsciiString(PiffBoxBase.BoxTypeLength, out boxId);

            if (status != PiffReadStatuses.Continue)
            {
                ctx.AddError($"Malformed file: reached EOF when reading box name at position {startPosition}. Aborting.");
                return PiffReadStatuses.EofPremature;
            }

            if (!boxId.All(c => char.IsLetterOrDigit(c) || c == ' '))
            {
                ctx.AddError($"Malformed box name at position {startPosition}. Skipping.");
                return PiffReadStatuses.SkipToEnd;
            }

            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read the 8-byte box length.
        /// </summary>
        private static PiffReadStatuses ReadBoxLength64(BitReadStream input, PiffReadContext ctx, long startPosition, string boxId, out ulong length)
        {
            var bytesInLen64 = input.Read(Length64Array, 0, sizeof(ulong));

            if (bytesInLen64 < sizeof(ulong))
            {
                ctx.AddError($"Unexpected EOF inside box '{boxId}' header at position {startPosition}.");
                length = 0;
                return PiffReadStatuses.EofPremature;
            }

            length = Length64Array.GetUInt64();
            return PiffReadStatuses.Continue;
        }

        #endregion
    }
}
