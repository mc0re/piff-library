﻿using System;
using System.Linq;


namespace PiffLibrary
{
    public static class PiffUtility
    {
        private static int[] sGuidByteOrder = new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };


        /// <summary>
        /// Convert GUID to big-endian (as opposed to <see cref="Guid.ToByteArray"/>).
        /// </summary>
        public static byte[] ToBigEndianArray(this Guid guid)
        {
            var guidBytes = guid.ToByteArray();

            return (from i in sGuidByteOrder select guidBytes[i]).ToArray();
        }
    }
}
