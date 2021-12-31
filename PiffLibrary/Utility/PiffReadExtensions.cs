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
        /// Read the given number of ASCII characters from a string.
        /// </summary>
        public static string GetFixedString(this Stream bytes, int length)
        {
            var chars = Enumerable.Range(0, length).Select(_ => (byte)bytes.ReadByte()).ToArray();

            return Encoding.ASCII.GetString(chars);
        }
    }
}
