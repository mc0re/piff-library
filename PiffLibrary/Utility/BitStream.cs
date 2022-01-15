using System;
using System.IO;


namespace PiffLibrary
{
    /// <summary>
    /// Stream allowing reading and writing of numbers taking up less than one byte.
    /// </summary>
    internal sealed class BitStream : IDisposable
    {
        #region Fields

        private readonly Stream mUnderlying;

        private readonly bool mDisposeUnderlying;

        private bool mIsDisposed;


        private static readonly int[] BitMask = { 0, 1, 0b11, 0b111, 0b1111, 0b11111, 0b111111, 0b1111111, 0b11111111 }; 


        /// <summary>
        /// The number of bits left unread in <see cref="mBitsStore"/>.
        /// </summary>
        private int mBitsLeft;
        

        /// <summary>
        /// Only used for less-than-a-byte reading.
        /// </summary>
        private int mBitsStore;

        #endregion


        public long Position => mUnderlying.Position;


        #region Init and clean-up

        public BitStream(Stream underlying, bool disposeUnderlying)
        {
            mUnderlying = underlying;
            mDisposeUnderlying = disposeUnderlying;
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
        /// Get the next byte or -1 if EOF.
        /// </summary>
        public int ReadByte()
        {
            return mUnderlying.ReadByte();
        }


        /// <summary>
        /// Read the specified number of bits.
        /// Reading is happening from left to right (high to low bits).
        /// The reading may not cross byte boundary.
        /// </summary>
        public int ReadBits(int nofBits)
        {
            if (nofBits <= 0 || nofBits > 7)
                throw new ArgumentException($"Can only read 1..7 bitsm requested {nofBits}.");

            if (mBitsLeft == 0)
            {
                mBitsStore = mUnderlying.ReadByte();
                mBitsLeft = mBitsStore < 0 ? 0 : 8;
            }

            if (mBitsLeft < nofBits)
                throw new ArgumentException($"Cannot read {nofBits} bits, only {mBitsLeft} is left.");

            var res = mBitsStore >> (mBitsLeft - nofBits);
            mBitsLeft -= nofBits;

            return res & BitMask[nofBits];
        }


        /// <summary>
        /// Read a number of bytes into the provided array.
        /// </summary>
        public int Read(byte[] buffer, int offset, int count)
        {
            return mUnderlying.Read(buffer, offset, count);
        }

        #endregion
    }
}