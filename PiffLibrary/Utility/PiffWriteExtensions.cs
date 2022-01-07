using System;
using System.Collections.Generic;
using System.Linq;


namespace PiffLibrary
{
    public static class PiffWriteExtensions
    {
        public static int[] GuidByteOrder = new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };


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
        /// <remarks>
        /// To simplify length calculation, always write 4 bytes.
        /// </remarks>
        public static IEnumerable<byte> ToDynamic(this int value)
        {
            var bytes = new byte[4];
            var idx = 3;

            while (value > 0)
            {
                var part = value & 0x7F;
                value >>= 7;
                bytes[idx] = (byte)part;
                idx--;
            }

            for (var setBitIdx = 0; setBitIdx < 3; setBitIdx++)
                bytes[setBitIdx] |= 0x80;

            return bytes;
        }
    }
}
