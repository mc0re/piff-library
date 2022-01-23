using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


[assembly:InternalsVisibleTo("PiffLibrary.Test")]


namespace PiffLibrary
{
    /// <summary>
    /// The methods here are not thread-safe due to use of static buffers.
    /// Switching to .NET Standard 2.1 or .NET 5+ would allow using Span<>
    /// and stackalloc, eliminating this problem.
    /// </summary>
    internal static class PiffReadExtensions
    {
        #region Static buffers

        private static readonly byte[] mInt16Buffer = new byte[sizeof(ushort)];

        private static readonly byte[] mInt24Buffer = new byte[3];

        private static readonly byte[] mInt32Buffer = new byte[sizeof(uint)];

        private static readonly byte[] mInt64Buffer = new byte[sizeof(ulong)];

        private static readonly byte[] mGuidBuffer = new byte[16];

        #endregion


        #region Read data from a byte array

        /// <summary>
        /// Read a 16-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static short GetInt16(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 8) |
                       bytes[offset + 1];

            return (short) res;
        }


        /// <summary>
        /// Read a 16-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static ushort GetUInt16(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 8) |
                       bytes[offset + 1];

            return (ushort) res;
        }


        /// <summary>
        /// Read a 24-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static int GetInt24(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 16) |
                      (bytes[offset + 1] << 8) |
                       bytes[offset + 2];

            return res;
        }


        /// <summary>
        /// Read a 32-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static int GetInt32(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 24) |
                      (bytes[offset + 1] << 16) |
                      (bytes[offset + 2] << 8) |
                       bytes[offset + 3];

            return res;
        }


        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static uint GetUInt32(this byte[] bytes, int offset = 0)
        {
            var res = (bytes[offset] << 24) |
                      (bytes[offset + 1] << 16) |
                      (bytes[offset + 2] << 8) |
                       bytes[offset + 3];

            return (uint) res;
        }


        /// <summary>
        /// Read a 64-bit signed integer in big-endian format from a byte array.
        /// </summary>
        public static long GetInt64(this byte[] bytes, int offset = 0)
        {
            return ((long) bytes.GetInt32(offset) << 32) |
                           bytes.GetUInt32(offset + 4);
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static ulong GetUInt64(this byte[] bytes, int offset = 0)
        {
            return ((ulong) bytes.GetUInt32(offset) << 32) |
                            bytes.GetUInt32(offset + 4);
        }

        #endregion


        #region Read data from a stream

        /// <summary>
        /// Read a 16-bit integer in big-endian format from a stream.
        /// </summary>
        internal static short ReadInt16(this BitReadStream bytes)
        {
            if (bytes.Read(mInt16Buffer, 0, mInt16Buffer.Length) < mInt16Buffer.Length)
                return BitReadStream.Eof;

            return mInt16Buffer.GetInt16();
        }


        /// <summary>
        /// Read a 16-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static ushort ReadUInt16(this BitReadStream bytes)
        {
            if (bytes.Read(mInt16Buffer, 0, mInt16Buffer.Length) < mInt16Buffer.Length)
                return 0xFFFF; // BitReadStream.Eof

            return mInt16Buffer.GetUInt16();
        }


        /// <summary>
        /// Read a 24-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadInt24(this BitReadStream bytes)
        {
            if (bytes.Read(mInt24Buffer, 0, mInt24Buffer.Length) < mInt24Buffer.Length)
                return BitReadStream.Eof;

            return mInt24Buffer.GetInt24();
        }


        /// <summary>
        /// Read a 32-bit integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadInt32(this BitReadStream bytes)
        {
            if (bytes.Read(mInt32Buffer, 0, mInt32Buffer.Length) < mInt32Buffer.Length)
                return BitReadStream.Eof;

            return mInt32Buffer.GetInt32();
        }


        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static uint ReadUInt32(this BitReadStream bytes)
        {
            if (bytes.Read(mInt32Buffer, 0, mInt32Buffer.Length) < mInt32Buffer.Length)
                return 0xFFFFFFFF; // BitReadStream.Eof

            return mInt32Buffer.GetUInt32();
        }


        /// <summary>
        /// Read a 64-bit integer in big-endian format from a stream.
        /// </summary>
        internal static long ReadInt64(this BitReadStream bytes)
        {
            if (bytes.Read(mInt64Buffer, 0, mInt64Buffer.Length) < mInt64Buffer.Length)
                return BitReadStream.Eof;

            return mInt64Buffer.GetInt64();
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static ulong ReadUInt64(this BitReadStream bytes)
        {
            if (bytes.Read(mInt64Buffer, 0, mInt64Buffer.Length) < mInt64Buffer.Length)
                return 0xFFFFFFFFFFFFFFFF; // BitReadStream.Eof

            return mInt64Buffer.GetUInt64();
        }


        /// <summary>
        /// Read 8..32-bit integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadDynamicInt(this BitReadStream bytes)
        {
            var res = 0;
            int b;

            do
            {
                b = bytes.ReadByte();
                res = (res << 7) | (b & 0x7F);
            }
             while (b > 0x7F);

            return res;
        }


        /// <summary>
        /// Read a 16-byte GUID in big-endian format from a stream.
        /// </summary>
        internal static Guid ReadGuid(this BitReadStream bytes)
        {
            if (bytes.Read(mGuidBuffer, 0, mGuidBuffer.Length) < mGuidBuffer.Length)
                return Guid.Empty;

            var fromBe = (from i in PiffWriteExtensions.GuidByteOrder select mGuidBuffer[i]).ToArray();
            return new Guid(fromBe);
        }


        /// <summary>
        /// Read a 0-terminated ASCII string.
        /// </summary>
        public static string ReadAsciiString(this BitReadStream bytes)
        {
            var str = new List<byte>();
            int b;

            while ((b = bytes.ReadByte()) > 0)
                str.Add((byte) b);

            return Encoding.ASCII.GetString(str.ToArray());
        }


        /// <summary>
        /// Read the given number of ASCII characters from a string.
        /// </summary>
        public static string ReadAsciiString(this BitReadStream bytes, int length)
        {
            var chars = Enumerable.Range(0, length)
                                  .Select(_ => bytes.ReadByte())
                                  .TakeWhile(c => c != BitReadStream.Eof)
                                  .Select(c => (byte)c)
                                  .ToArray();

            return Encoding.ASCII.GetString(chars);
        }


        /// <summary>
        /// Read a 0-terminated UTF8 string.
        /// </summary>
        public static string ReadUtf8String(this BitReadStream bytes)
        {
            var str = new List<byte>();
            int b;

            while ((b = bytes.ReadByte()) > 0)
                str.Add((byte) b);

            return Encoding.UTF8.GetString(str.ToArray());
        }


        /// <summary>
        /// Read a 0-terminated UTF-8 or UTF-16 string.
        /// UTF-16 must start with byte order mark 0xFEFF.
        /// </summary>
        public static string ReadUtf8Or16String(this BitReadStream bytes)
        {
            var str = new List<byte>();
            int b;

            // EOF = -1, end-of-string = 0
            while ((b = bytes.ReadByte()) > 0)
                str.Add((byte) b);

            if (str.Count > 2 && str[0] == 0xFE && str[1] == 0xFF)
                return new UnicodeEncoding(true, true).GetString(str.ToArray());

            return Encoding.UTF8.GetString(str.ToArray());
        }

        #endregion
    }
}
