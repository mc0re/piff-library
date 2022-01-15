using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace PiffLibrary.Test.Infrastructure
{
    [TestClass]
    public class BitWriteStreamTests
    {
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
        public void BitStream_WriteBits()
        {
            var ms = new MemoryStream();
            var sut = new BitWriteStream(ms, true);

            sut.WriteBits(1, 1);
            sut.WriteBits(2, 2);
            sut.WriteBits(0x1B, 5);
            sut.WriteBits(12, 4);
            sut.WriteBits(0, 1);
            sut.WriteBits(5, 3);

            Assert.AreEqual(2, sut.Position);
            var buf = ms.GetBuffer().Take((int) ms.Length).ToArray();
            TestUtil.Compare(new byte[] { 0b11011011, 0b11000101 }, buf);
        }


        [TestMethod]
        public void BitStream_WriteBeyondBoundary()
        {
            var ms = new MemoryStream();
            var sut = new BitWriteStream(ms, true);

            sut.WriteBits(0x1B, 5);
            Assert.ThrowsException<ArgumentException>(() => sut.WriteBits(1, 5));
        }
    }
}
