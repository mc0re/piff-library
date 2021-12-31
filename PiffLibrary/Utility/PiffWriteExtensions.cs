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
        /// Convert an integer to big-endian 4 bytes.
        /// </summary>
        public static IEnumerable<byte> ToBigEndian(this int value)
        {
            IEnumerable<byte> bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse();

            return bytes;
        }
    }
}
