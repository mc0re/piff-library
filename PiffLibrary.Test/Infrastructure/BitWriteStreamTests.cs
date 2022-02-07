using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace PiffLibrary.Test.Infrastructure
{
    [TestClass]
    public class BitWriteStreamTests
    {
        #region BitStream tests

        [TestMethod]
        public void BitStream_WriteByte()
        {
            var ms = new MemoryStream();
            var sut = new BitWriteStream(ms, true);

            sut.WriteByte(0x12);
            sut.WriteByte(0x34);

            Assert.AreEqual(2, sut.Position);
            var buf = ms.GetBuffer().Take((int) ms.Length).ToArray();
            TestUtil.Compare(new byte[] { 0x12, 0x34 }, buf);
        }


        [TestMethod]
        public void BitStream_WriteBitsAligned()
        {
            var ms = new MemoryStream();
            var sut = new BitWriteStream(ms, true);

            sut.WriteBits(1, 1);
            sut.WriteBits(0b10, 2);
            sut.WriteBits(0b11011, 5);
            sut.WriteBits(0b1100, 4);
            sut.WriteBits(0, 1);
            sut.WriteBits(0b101, 3);

            Assert.AreEqual(2, sut.Position);
            var buf = ms.GetBuffer().Take((int) ms.Length).ToArray();
            TestUtil.Compare(new byte[] { 0b1_10_11011, 0b1100_0_101 }, buf);
        }


        [TestMethod]
        public void BitStream_WriteBeyondBoundary()
        {
            var ms = new MemoryStream();
            var sut = new BitWriteStream(ms, true);

            sut.WriteBits(0x1B, 5);
            Assert.ThrowsException<ArgumentException>(() => sut.WriteBits(1, 5));
        }

        #endregion


        #region Extension tests

        [TestMethod]
        public void BitStream_WriteDynamicInt()
        {
            var b0 = 0u.ToDynamic().ToArray();
            TestUtil.Compare(new byte[] { 0 }, b0);

            var b1 = 0x12u.ToDynamic().ToArray();
            TestUtil.Compare(new byte[] { 0x12 }, b1);

            var b2 = 0xd3u.ToDynamic().ToArray();
            TestUtil.Compare(new byte[] { 0x81, 0x53 }, b2);
        }

        #endregion
    }
}
