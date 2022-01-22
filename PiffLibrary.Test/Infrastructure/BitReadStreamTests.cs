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

            Assert.AreEqual(1, sut.ReadBits(1, false));
            Assert.AreEqual(2, sut.ReadBits(2, false));
            Assert.AreEqual(0x1B, sut.ReadBits(5, false));
            Assert.AreEqual(12, sut.ReadBits(4, false));
            Assert.AreEqual(0, sut.ReadBits(1, false));
            Assert.AreEqual(5, sut.ReadBits(3, false));
            Assert.ThrowsException<EndOfStreamException>(() => sut.ReadBits(1, false));
        }


        [TestMethod]
        public void BitStream_ReadBitsAcrossBoundary()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011_011, 0b1_1010101, 0b01010_101 }), true);

            Assert.AreEqual(0x1B, sut.ReadBits(5, false));
            Assert.AreEqual(0x07, sut.ReadBits(4, false));
            Assert.AreEqual(0x0AAA, sut.ReadBits(12, false));
        }


        [TestMethod]
        public void BitStream_ReadSignedBits()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11111111, 0b11111000 }), true);

            Assert.AreEqual(-1, sut.ReadBits(12, true));
            Assert.AreEqual(-8, sut.ReadBits(4, true));
        }


        [TestMethod]
        public void BitStream_ReadByteUnaligned()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011011, 0b11000101 }), true);

            Assert.AreEqual(0x1B, sut.ReadBits(5, false));
            Assert.ThrowsException<ArgumentException>(() => sut.ReadByte());
        }


        [TestMethod]
        public void BitStream_ReadTooManyBits()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011011 }), true);

            Assert.ThrowsException<ArgumentException>(() => sut.ReadBits(32, false));
        }
    }
}
