using System;
using System.IO;


namespace PiffLibrary
{
    /// <summary>
    /// Stream allowing reading and writing of numbers taking up less than 1 byte.
    /// </summary>
    internal sealed class BitStream : IDisposable
    {
        #region Fields

        private readonly Stream mUnderlying;

        private readonly bool mDisposeUnderlying;

        private bool mIsDisposed;

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
        /// Read a number of bytes into the provided array.
        /// </summary>
        public int Read(byte[] buffer, int offset, int count)
        {
            return mUnderlying.Read(buffer, offset, count);
        }

        #endregion
    }
}