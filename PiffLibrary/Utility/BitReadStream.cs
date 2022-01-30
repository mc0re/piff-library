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

        public const int Eof = -1;

        /// <summary>
        /// The number of bits in a byte.
        /// </summary>
        private const int ByteSize = 8;
        
        /// <summary>
        /// The maximum number of bits we can read or write in one go using <see cref="ReadBits"/>.
        /// </summary>
        private const int MaxBits = sizeof(uint) * ByteSize - 1;

        #endregion


        #region Fields

        private readonly Stream mUnderlying;

        private readonly bool mDisposeUnderlying;

        private bool mIsDisposed;


        /// <summary>
        /// To extract the needed bits from a byte.
        /// Array index is the number of needed bits.
        /// </summary>
        private static readonly int[] BitsMask = { 0, 0b1, 0b11, 0b111, 0b1111, 0b11111, 0b111111, 0b1111111, 0b11111111 }; 


        /// <summary>
        /// To propagate negative sign.
        /// </summary>
        private static readonly uint[] SignMask;


        /// <summary>
        /// The number of bits left unread in <see cref="mBitsStore"/>.
        /// </summary>
        private int mBitsLeft;
        

        /// <summary>
        /// Only used for unaligned bits reading.
        /// </summary>
        private int mBitsStore;

        #endregion


        #region Properties

        public long Position => mUnderlying.Position;

        private ulong mBytesLeft;

        public ulong BytesLeft => mBytesLeft;

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


        public BitReadStream(Stream underlying, bool disposeUnderlying)
        {
            mUnderlying = underlying;
            mBytesLeft = (ulong) underlying.Length;
            mDisposeUnderlying = disposeUnderlying;
        }

        /// <summary>
        /// Create a slice of the <paramref name="underlying"/> stream
        /// from its current position and <paramref name="length"/> bytes long.
        /// </summary>
        public BitReadStream(BitReadStream underlying, ulong length)
        {
            mUnderlying = underlying.mUnderlying;
            mBytesLeft = length;
        }

        #endregion


        #region API

        public void Dispose()
        {
            if (!mIsDisposed)
            {
                if (mDisposeUnderlying)
                    mUnderlying.Dispose();
    
                mIsDisposed = true;
            }
        }


        public void Seek(int offset, SeekOrigin origin)
        {
            mUnderlying.Seek(offset, origin);
        }


        /// <summary>
        /// Get the next byte or <see cref="Eof"/> if EOF.
        /// </summary>
        public int ReadByte()
        {
            if (mBitsLeft != 0)
                throw new ArgumentException("Reading unaligned byte. Please check your layout.");

            if (mBytesLeft == 0) return Eof;

            var read = mUnderlying.ReadByte();
            if (read >= 0)
                mBytesLeft--;
            else
                mBytesLeft = 0;

            return read;
        }


        /// <summary>
        /// Read the specified number of bits.
        /// Reading is happening from left to right (high to low bits).
        /// If <paramref name="isSigned"/> is set, the sign bit gets duplicated to the left.
        /// </summary>
        public int ReadBits(int nofBits, bool isSigned)
        {
            if (nofBits <= 0 || nofBits > MaxBits)
                throw new ArgumentException($"Can only read 1..{MaxBits} bits, requested {nofBits}.");

            var res = 0;
            var bitsToReadLeft = nofBits;

            while (bitsToReadLeft > 0)
            {
                if (mBitsLeft == 0)
                {
                    // Read next full byte
                    mBitsStore = ReadByte();
                    if (mBitsStore < 0) throw new EndOfStreamException($"Cannot read next byte.");

                    mBitsLeft = ByteSize;
                }

                var bitsToReadNow = bitsToReadLeft > mBitsLeft ? mBitsLeft : bitsToReadLeft;
                
                res = (res << bitsToReadNow) | (mBitsStore >> (mBitsLeft - bitsToReadNow)) & BitsMask[bitsToReadNow];
                mBitsLeft -= bitsToReadNow;
                bitsToReadLeft -= bitsToReadNow;
            }

            if (isSigned && (res & (1 << (nofBits - 1))) != 0)
            {
                res = (int) ((uint)res | SignMask[nofBits]);
            }

            return res;
        }


        /// <summary>
        /// Read a number of bytes into the provided array.
        /// </summary>
        public int Read(byte[] buffer, int offset, int count)
        {
            var toRead = Math.Min((ulong)count, mBytesLeft);
            var len = mUnderlying.Read(buffer, offset, (int) toRead);

            if ((ulong) len == toRead)
                mBytesLeft -= (uint) len;
            else
                mBytesLeft = 0;

            return len;
        }

        #endregion
    }
}