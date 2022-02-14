using System;
using System.Collections.Generic;
using System.Linq;


namespace PiffLibrary
{
    internal static class PiffWriteExtensions
    {
        public static int[] GuidByteOrder = new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };


        #region Generate bytes

        /// <summary>
        /// Convert GUID to big-endian (as opposed to <see cref="Guid.ToByteArray"/>).
        /// </summary>
        public static byte[] ToBigEndianArray(this Guid guid)
        {
            var guidBytes = guid.ToByteArray();

            return (from i in GuidByteOrder select guidBytes[i]).ToArray();
        }


        /// <summary>
        /// Convert an integer to big-endian 2 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this short value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Convert an integer to big-endian 2 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this ushort value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Convert an integer to big-endian 4 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this int value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Convert an integer to big-endian 4 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this uint value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Convert an integer to big-endian 8 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this long value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Convert an integer to big-endian 8 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this ulong value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }


        /// <summary>
        /// Write multiple bytes, with highest bit signifying that another byte follows.
        /// </summary>
        public static IEnumerable<byte> ToDynamic(this uint value)
        {
            var numBytes = (int) value.ToDynamicLen();
            var idx = numBytes;
            var bytes = new byte[numBytes];

            while (value > 0)
            {
                idx--;
                var part = value & 0x7F;
                value >>= 7;
                bytes[idx] = (byte)part;
            }

            for (var setBitIdx = 0; setBitIdx < numBytes - 1; setBitIdx++)
                bytes[setBitIdx] |= 0x80;

            return bytes;
        }


        /// <summary>
        /// Calculate the number of bytes needed to store the number as dynamic.
        /// </summary>
        public static uint ToDynamicLen(this uint value)
        {
            return value > 0b1111111_1111111_1111111 ? 4u :
                   value > 0b1111111_1111111 ? 3u :
                   value > 0b1111111 ? 2u :
                   1u;
        }

        #endregion


        #region Stream writes

        public static void WriteBytes(this BitWriteStream output, IEnumerable<byte> bytes)
        {
            var arr = bytes.ToArray();
            output.Write(arr, 0, arr.Length);
        }

        #endregion
    }
}
