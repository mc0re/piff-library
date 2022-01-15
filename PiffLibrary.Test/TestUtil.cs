using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;


namespace PiffLibrary.Test
{
    internal static class TestUtil
    {
        public static void Compare(byte[] expected, byte[] actual, params int[] skipAddr)
        {
            for (var i = 0; i < actual.Length; i++)
            {
                // During debugging - can skip some addresses to get to the data
                if (skipAddr.Contains(i)) continue;

                if (i >= expected.Length)
                    Assert.Fail($"Expected length {expected.Length}, actual {actual.Length}.");

                Assert.AreEqual(expected[i], actual[i], $"Index 0x{i:X}: expected 0x{expected[i]:X2}, actual 0x{actual[i]:X2}");
            }

            Assert.AreEqual(expected.Length, actual.Length, $"Expected length {expected.Length}, actual {actual.Length}.");
        }
    }
}
