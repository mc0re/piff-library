using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;


namespace PiffLibrary.Test.Infrastructure
{
    [TestClass]
    public class BitReadStreamTests
    {
        [TestMethod]
        public void BitStream_ReadByte()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 2, 3 }), true);

            Assert.AreEqual(2, sut.ReadByte());
            Assert.AreEqual(3, sut.ReadByte());
            Assert.AreEqual(-1, sut.ReadByte());
        }


        [TestMethod]
        public void BitStream_ReadBlock()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 12, 13, 14, 15 }), true);

            var buf = new byte[3];
            Assert.AreEqual(3, sut.Read(buf, 0, 3));
            Assert.AreEqual(12, buf[0]);
            Assert.AreEqual(13, buf[1]);
            Assert.AreEqual(14, buf[2]);
            Assert.AreEqual(15, sut.ReadByte());
            Assert.AreEqual(-1, sut.ReadByte());
        }


        [TestMethod]
        public void BitStream_ReadBitsAligned()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011011, 0b11000101 }), true);

            Assert.AreEqual(1, sut.ReadBits(1));
            Assert.AreEqual(2, sut.ReadBits(2));
            Assert.AreEqual(0x1B, sut.ReadBits(5));
            Assert.AreEqual(12, sut.ReadBits(4));
            Assert.AreEqual(0, sut.ReadBits(1));
            Assert.AreEqual(5, sut.ReadBits(3));
            Assert.AreEqual(-1, sut.ReadByte());
        }


        [TestMethod]
        public void BitStream_ReadBitsAcrossBoundary()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011011 }), true);

            Assert.AreEqual(0x1B, sut.ReadBits(5));
            Assert.ThrowsException<ArgumentException>(() => sut.ReadBits(5));
        }
    }
}
