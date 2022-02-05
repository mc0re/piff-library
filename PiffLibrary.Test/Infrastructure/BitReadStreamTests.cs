using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;


namespace PiffLibrary.Test.Infrastructure
{

    [TestClass]
    public class BitReadStreamTests
    {
        #region BitStream tests

        [TestMethod]
        public void BitStream_ReadByte()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 2, 3 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadByte(out var b0));
            Assert.AreEqual(2, b0);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadByte(out var b1));
            Assert.AreEqual(3, b1);
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadByte(out _));
        }


        [TestMethod]
        public void BitStream_ReadByteMisconfig()
        {
            var ms = new MemoryStream(new byte[] { 2, 3 });
            using var sut = new BitReadStream(ms, true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadByte(out var b0));
            Assert.AreEqual(2, b0);

            ms.SetLength(1);
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadByte(out _));
        }


        [TestMethod]
        public void BitStream_ReadByteSeek()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 2, 3 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadByte(out var b0));
            Assert.AreEqual(2, b0);
            Assert.AreEqual(1uL, sut.BytesLeft);

            Assert.IsTrue(sut.Seek(1uL));
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadByte(out _));
            Assert.AreEqual(0uL, sut.BytesLeft);

            Assert.IsFalse(sut.Seek(1uL));
            Assert.AreEqual(0uL, sut.BytesLeft);
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
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadByte(out var b3));
            Assert.AreEqual(15, b3);
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadByte(out _));
        }


        [TestMethod]
        public void BitStream_ReadBlockMisconfig()
        {
            var ms = new MemoryStream(new byte[] { 12, 13, 14, 15 });
            using var sut = new BitReadStream(ms, true);

            var buf = new byte[2];
            Assert.AreEqual(2, sut.Read(buf, 0, 2));
            Assert.AreEqual(12, buf[0]);
            Assert.AreEqual(13, buf[1]);

            ms.ReadByte();

            Assert.AreEqual(2uL, sut.BytesLeft);
            Assert.AreEqual(1, sut.Read(buf, 0, 2));
            Assert.AreEqual(15, buf[0]);
            Assert.AreEqual(0uL, sut.BytesLeft);
        }


        [TestMethod]
        public void BitStream_ReadBitsAligned()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b1_10_11011, 0b1100_0_101 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(1, false, out var b1));
            Assert.AreEqual(1, b1);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(2, false, out var b2));
            Assert.AreEqual(2, b2);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(5, false, out var b3));
            Assert.AreEqual(0x1B, b3);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(4, false, out var b4));
            Assert.AreEqual(12, b4);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(1, false, out var b5));
            Assert.AreEqual(0, b5);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(3, false, out var b6));
            Assert.AreEqual(5, b6);
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadBits(1, false, out _));
        }


        [TestMethod]
        public void BitStream_ReadBitsAcrossBoundary()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011_011, 0b1_1010101, 0b01010_101 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(5, false, out var b1));
            Assert.AreEqual(0x1B, b1);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(4, false, out var b2));
            Assert.AreEqual(0x07, b2);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(12, false, out var b3));
            Assert.AreEqual(0x0AAA, b3);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(2, false, out var b4));
            Assert.AreEqual(2, b4);
            Assert.AreEqual(PiffReadStatuses.EofPremature, sut.ReadBits(2, false, out _));
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadBits(2, false, out _));
        }


        [TestMethod]
        public void BitStream_ReadSignedBits()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11111111, 0b11111000 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(12, true, out var b1));
            Assert.AreEqual(-1, b1);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(4, true, out var b2));
            Assert.AreEqual(-8, b2);
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadBits(1, false, out _));
        }


        [TestMethod]
        public void BitStream_ReadByteUnaligned()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011011, 0b11000101 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadBits(5, false, out var b1));
            Assert.AreEqual(0x1B, b1);
            Assert.ThrowsException<ArgumentException>(() => sut.ReadByte(out _));
        }


        [TestMethod]
        public void BitStream_ReadTooManyBits()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0b11011011 }), true);

            Assert.ThrowsException<ArgumentException>(() => sut.ReadBits(32, false, out _));
        }


        [TestMethod]
        public void BitStream_CreateSlice()
        {
            using var main = new BitReadStream(new MemoryStream(new byte[] { 2, 3, 4 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, main.ReadByte(out var b0));
            Assert.AreEqual(2, b0);

            using var sut = new BitReadStream(main, 1, "sub-stream");

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadByte(out var b1));
            Assert.AreEqual(3, b1);
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadByte(out _));

            main.Consolidate(sut);

            Assert.AreEqual(PiffReadStatuses.Continue, main.ReadByte(out var b2));
            Assert.AreEqual(4, b2);
            Assert.AreEqual(PiffReadStatuses.Eof, main.ReadByte(out _));
        }

        #endregion
    }
}
