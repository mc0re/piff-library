using System;
using System.IO;


namespace PiffLibrary
{
    /// <summary>
    /// Stream allowing writing of numbers taking up less than one byte.
    /// </summary>
    internal sealed class BitWriteStream : IDisposable
    {
        #region Fields

        private readonly Stream mUnderlying;

        private readonly bool mDisposeUnderlying;

        private bool mIsDisposed;


        private const int ByteSize = 8;

        private static readonly int[] BitMask = { 0, 1, 0b11, 0b111, 0b1111, 0b11111, 0b111111, 0b1111111, 0b11111111 }; 


        /// <summary>
        /// The number of bits available for writing in <see cref="mBitsStore"/>.
        /// </summary>
        private int mBitsFree = ByteSize;
        

        /// <summary>
        /// Only used for less-than-a-byte writing.
        /// </summary>
        private int mBitsStore;

        #endregion


        #region Properties

        public long Position => mUnderlying.Position;

        #endregion


        #region Init and clean-up

        public BitWriteStream(Stream underlying, bool disposeUnderlying)
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


        public void WriteByte(byte value)
        {
            mUnderlying.WriteByte(value);
        }


        /// <summary>
        /// Write the (unsigned) value in given number of bits.
        /// Writing happens left to right (high to low bits).
        /// Writing cannot cross the byte boundary.
        /// </summary>
        public void WriteBits(int value, int nofBits)
        {
            if (nofBits <= 0 || nofBits > 7)
                throw new ArgumentException($"Can only write 1..7 bits, requested {nofBits}.");

            if (mBitsFree < nofBits)
                throw new ArgumentException($"Cannot write {nofBits} bits, only {mBitsFree} bits left.");

            mBitsStore |= (value << (mBitsFree - nofBits));
            mBitsFree -= nofBits;

            if (mBitsFree == 0)
            {
                // Flush the store
                mUnderlying.WriteByte((byte) mBitsStore);
                mBitsStore = 0;
                mBitsFree = ByteSize;
            }
        }


        /// <summary>
        /// Write a number of bytes from the given buffer.
        /// </summary>
        public void Write(byte[] buffer, int offset, int length)
        {
            mUnderlying.Write(buffer, offset, length);
        }


        /// <summary>
        /// Write all bytes of <paramref name="substream"/> into <see langword="this"/> stream.
        /// </summary>
        public void Consolidate(BitWriteStream substream)
        {
            if (!substream.mUnderlying.CanSeek || !substream.mUnderlying.CanRead)
                throw new ArgumentException("Cannot copy data from this stream.");

            substream.mUnderlying.Seek(0, SeekOrigin.Begin);
            substream.mUnderlying.CopyTo(mUnderlying);
        }

        #endregion
    }
}