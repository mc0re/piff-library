using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace PiffLibrary
{
    internal static class PiffReadExtensions
    {
        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a byte array.
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
        /// Read a 16-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadInt16(this Stream bytes)
        {
            var res = (bytes.ReadByte() << 8) |
                       bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read a 24-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadInt24(this Stream bytes)
        {
            var res = (bytes.ReadByte() << 16) |
                      (bytes.ReadByte() << 8) |
                       bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read a 32-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static uint ReadUInt32(this Stream bytes)
        {
            var res = ((uint) bytes.ReadByte() << 24) |
                      ((uint) bytes.ReadByte() << 16) |
                      ((uint) bytes.ReadByte() << 8) |
                       (uint) bytes.ReadByte();

            return res;
        }


        /// <summary>
        /// Read a 64-bit unsigned integer in big-endian format from a stream.
        /// </summary>
        internal static ulong ReadUInt64(this Stream bytes)
        {
            var res = ((ulong)ReadUInt32(bytes) << 32) | ReadUInt32(bytes);

            return res;
        }


        /// <summary>
        /// Read 8..32-bit integer in big-endian format from a stream.
        /// </summary>
        internal static int ReadDynamicInt(this Stream bytes)
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
        internal static Guid ReadGuid(this Stream bytes)
        {
            var arr = new byte[16];
            bytes.Read(arr, 0, arr.Length);
            var fromBe = (from i in PiffWriteExtensions.GuidByteOrder select arr[i]).ToArray();
            return new Guid(fromBe);
        }


        /// <summary>
        /// Read the given number of ASCII characters from a string.
        /// </summary>
        public static string ReadAsciiString(this Stream bytes, int length)
        {
            var chars = Enumerable.Range(0, length).Select(_ => (byte)bytes.ReadByte()).ToArray();

            return Encoding.ASCII.GetString(chars);
        }


        /// <summary>
        /// Read a 0-terminated UTF8 string.
        /// </summary>
        public static string ReadUtf8String(this Stream bytes)
        {
            var str = new List<byte>();
            byte b;

            while ((b = (byte)bytes.ReadByte()) != 0)
                str.Add(b);

            return Encoding.UTF8.GetString(str.ToArray());
        }
    }
}
