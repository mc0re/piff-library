using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


[assembly:InternalsVisibleTo("PiffLibrary.Test")]


namespace PiffLibrary
{
    internal static class PiffReadExtensions
    {
        #region Read data from a byte array

        /// <summary>
        /// Read a 32-bit integer in big-endian format from a byte array.
        /// </summary>
        public static int GetInt32(this byte[] bytes, int offset)
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
        public static uint GetUInt32(this byte[] bytes, int offset)
        {
            var res = (bytes[offset] << 24) |
                      (bytes[offset + 1] << 16) |
                      (bytes[offset + 2] << 8) |
                       bytes[offset + 3];

            return (uint) res;
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a byte array.
        /// </summary>
        public static ulong GetUInt64(this byte[] bytes, int offset)
        {
            var res = (ulong)(bytes.GetUInt32(offset) << 32) |
                              bytes.GetUInt32(offset + 4);

            return (uint) res;
        }

        #endregion


        #region Read data from a stream

        /// <summary>
        /// Read a 16-bit integer in big-endian format from a stream.
        /// </summary>
        internal static short ReadInt16(this BitReadStream bytes)
        {
            var res = (bytes.ReadByte() << 8) |
                       bytes.ReadByte();

            return (short)res;
        }


        /// <summary>
        /// Read a 16-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static ushort ReadUInt16(this BitReadStream bytes)
        {
            var res = (bytes.ReadByte() << 8) |
                       bytes.ReadByte();

            return (ushort)res;
        }


        /// <summary>
        /// Read a 24-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadInt24(this BitReadStream bytes)
        {
            var res = (bytes.ReadByte() << 16) |
                      (bytes.ReadByte() << 8) |
                       bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read a 32-bit integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadInt32(this BitReadStream bytes)
        {
            var res = (bytes.ReadByte() << 24) |
                      (bytes.ReadByte() << 16) |
                      (bytes.ReadByte() << 8) |
                       bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static uint ReadUInt32(this BitReadStream bytes)
        {
            var res = ((uint) bytes.ReadByte() << 24) |
                      ((uint) bytes.ReadByte() << 16) |
                      ((uint) bytes.ReadByte() << 8) |
                       (uint) bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read a 64-bit integer in big-endian format from a stream.
        /// </summary>
        internal static long ReadInt64(this BitReadStream bytes)
        {
            var res = ((long)bytes.ReadInt32() << 32) | bytes.ReadUInt32();

            return res;
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static ulong ReadUInt64(this BitReadStream bytes)
        {
            var res = ((ulong)bytes.ReadUInt32() << 32) | bytes.ReadUInt32();

            return res;
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
            var arr = new byte[16];
            bytes.Read(arr, 0, arr.Length);
            var fromBe = (from i in PiffWriteExtensions.GuidByteOrder select arr[i]).ToArray();
            return new Guid(fromBe);
        }


        /// <summary>
        /// Read a 0-terminated ASCII string.
        /// </summary>
        public static string ReadAsciiString(this BitReadStream bytes)
        {
            var str = new List<byte>();
            byte b;

            while ((b = (byte)bytes.ReadByte()) != 0)
                str.Add(b);

            return Encoding.ASCII.GetString(str.ToArray());
        }


        /// <summary>
        /// Read the given number of ASCII characters from a string.
        /// </summary>
        public static string ReadAsciiString(this BitReadStream bytes, int length)
        {
            var chars = Enumerable.Range(0, length).Select(_ => (byte)bytes.ReadByte()).ToArray();

            return Encoding.ASCII.GetString(chars);
        }


        /// <summary>
        /// Read a 0-terminated UTF8 string.
        /// </summary>
        public static string ReadUtf8String(this BitReadStream bytes)
        {
            var str = new List<byte>();
            byte b;

            while ((b = (byte)bytes.ReadByte()) != 0)
                str.Add(b);

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
