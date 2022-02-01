using System;
using System.Collections.Generic;
using System.IO;


namespace PiffLibrary
{
    /// <summary>
    /// Stream allowing reading of numbers taking up less than one byte.
    /// </summary>
    internal sealed class BitReadStream : IDisposable
    {
        #region Constants

        /// <summary>
        /// The number of bits in a byte.
        /// </summary>
        private const int ByteSize = 8;
        
        /// <summary>
        /// The maximum number of bits we can read or write in one go using <see cref="ReadBits"/>.
        /// </summary>
        private const int MaxBits = sizeof(uint) * ByteSize - 1;


        /// <summary>
        /// To extract the needed bits from a byte.
        /// Array index is the number of needed bits.
        /// </summary>
        private static readonly int[] BitsMask = { 0, 0b1, 0b11, 0b111, 0b1111, 0b11111, 0b111111, 0b1111111, 0b11111111 }; 

        #endregion


        #region Fields

        /// <summary>
        /// To propagate negative sign.
        /// </summary>
        private static readonly uint[] SignMask;


        /// <summary>
        /// The existing stream providing the data.
        /// </summary>
        private readonly Stream mUnderlying;


        /// <summary>
        /// Whether we need to dispose the <see cref="mUnderlying"/> stream
        /// when calling <see cref="Dispose"/>.
        /// </summary>
        private readonly bool mDisposeUnderlying;


        /// <summary>
        /// Whether this stream has been disposed (to allow multiple calls).
        /// </summary>
        private bool mIsDisposed;


        /// <summary>
        /// For debugging.
        /// </summary>
        private readonly string mName;


        /// <summary>
        /// The number of bits left unread in <see cref="mBitsStore"/>.
        /// </summary>
        private int mBitsLeft;
        

        /// <summary>
        /// Only used for unaligned bits reading.
        /// </summary>
        private byte mBitsStore;

        #endregion


        #region Properties

        /// <summary>
        /// Current byte position from the start of the original stream.
        /// </summary>
        public long Position => mUnderlying.Position;


        /// <summary>
        /// How many bytes are left in this stream or slice.
        /// </summary>
        public ulong BytesLeft { get; private set; }

        #endregion


        #region Init and clean-up

        static BitReadStream()
        {
            var list = new List<uint>();
            var mask = 0xFFFFFFFFu;

            for (var i = 0; i <= MaxBits; i++)
            {
                list.Add(mask);
                mask <<= 1;
            }

            SignMask = list.ToArray();
        }


        /// <summary>
        /// Create a stream upon an existing stream (e.g. file or memory).
        /// </summary>
        /// <param name="underlying">The existing stream providing the data.</param>
        /// <param name="disposeUnderlying">
        /// Whether we need to dispose the <paramref name="underlying"/> stream
        /// when calling <see cref="Dispose"/>.
        /// </param>
        public BitReadStream(Stream underlying, bool disposeUnderlying)
        {
            mUnderlying = underlying;
            BytesLeft = (ulong) underlying.Length;
            mDisposeUnderlying = disposeUnderlying;
        }

        /// <summary>
        /// Create a slice of the <paramref name="underlying"/> stream
        /// from its current position and <paramref name="length"/> bytes long.
        /// </summary>
        public BitReadStream(BitReadStream underlying, ulong length, string name = "")
        {
            mUnderlying = underlying.mUnderlying;
            BytesLeft = length;
            mName = name;
        }


        public void Dispose()
        {
            if (!mIsDisposed)
            {
                if (mDisposeUnderlying)
                    mUnderlying.Dispose();
    
                mIsDisposed = true;
            }
        }

        #endregion


        #region API
        
        /// <summary>
        /// Move the internal position by <paramref name="offset"/> bytes.
        /// The underlying stream was already moved.
        /// </summary>
        public void Advance(ulong offset)
        {
            BytesLeft -= offset;
        }


        /// <summary>
        /// Move the read position forward.
        /// </summary>
        public void Seek(ulong offset)
        {
            mUnderlying.Seek((long) offset, SeekOrigin.Current);
            BytesLeft -= offset;
        }


        /// <summary>
        /// Get the next byte.
        /// </summary>
        public PiffReadStatuses ReadByte(out byte result)
        {
            if (mBitsLeft != 0)
                throw new ArgumentException("Reading unaligned byte. Please check your layout.");

            if (BytesLeft == 0)
            {
                // Slice boundary
                result = 0;
                return PiffReadStatuses.Eof;
            }

            var read = mUnderlying.ReadByte();

            if (read >= 0)
            {
                result = (byte) read;
                BytesLeft--;
                return PiffReadStatuses.Continue;
            }
            else
            {
                result = 0;
                BytesLeft = 0;
                return PiffReadStatuses.Eof;
            }
        }


        /// <summary>
        /// Read the specified number of bits.
        /// Reading is happening from left to right (high to low bits).
        /// If <paramref name="isSigned"/> is set, the sign bit gets duplicated to the left.
        /// </summary>
        public PiffReadStatuses ReadBits(int nofBits, bool isSigned, out int result)
        {
            if (nofBits <= 0 || nofBits > MaxBits)
                throw new ArgumentException($"Can only read 1..{MaxBits} bits, requested {nofBits}.");

            result = 0;
            var bitsToReadLeft = nofBits;

            while (bitsToReadLeft > 0)
            {
                if (mBitsLeft == 0)
                {
                    // Read next full byte
                    var status = ReadByte(out mBitsStore);
                    if (status != PiffReadStatuses.Continue) return status;

                    mBitsLeft = ByteSize;
                }

                var bitsToReadNow = bitsToReadLeft > mBitsLeft ? mBitsLeft : bitsToReadLeft;
                
                result = (result << bitsToReadNow) | (mBitsStore >> (mBitsLeft - bitsToReadNow)) & BitsMask[bitsToReadNow];
                mBitsLeft -= bitsToReadNow;
                bitsToReadLeft -= bitsToReadNow;
            }

            if (isSigned && (result & (1 << (nofBits - 1))) != 0)
            {
                result = (int) ((uint)result | SignMask[nofBits]);
            }

            return PiffReadStatuses.Continue;
        }


        /// <summary>
        /// Read a number of bytes into the provided array.
        /// </summary>
        public int Read(byte[] buffer, int offset, int count)
        {
            var toRead = Math.Min((ulong)count, BytesLeft);
            var len = mUnderlying.Read(buffer, offset, (int) toRead);

            if ((ulong) len == toRead)
                BytesLeft -= (uint) len;
            else
                BytesLeft = 0;

            return len;
        }

        #endregion
    }
}