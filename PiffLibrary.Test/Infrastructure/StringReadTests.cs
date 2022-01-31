using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace PiffLibrary.Test.Infrastructure
{
    [TestClass]
    public class StringReadTests
    {
        [TestMethod]
        public void StringRead_AsciiZero()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0x41, 0x62, 0, 0x43, 0, 0, 0x44 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadAsciiZeroString(out var s1));
            Assert.AreEqual("Ab", s1);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadAsciiZeroString(out var s2));
            Assert.AreEqual("C", s2);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadAsciiZeroString(out var s3));
            Assert.AreEqual("", s3);
            Assert.AreEqual(PiffReadStatuses.EofPremature, sut.ReadAsciiZeroString(out _));
            Assert.AreEqual(PiffReadStatuses.Eof, sut.ReadAsciiZeroString(out _));
        }


        [TestMethod]
        public void StringRead_Ascii()
        {
            using var sut = new BitReadStream(new MemoryStream(new byte[] { 0x41, 0x62, 0x43, 0x44 }), true);

            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadAsciiString(2, out var s1));
            Assert.AreEqual("Ab", s1);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadAsciiString(1, out var s2));
            Assert.AreEqual("C", s2);
            Assert.AreEqual(PiffReadStatuses.Continue, sut.ReadAsciiString(0, out var s3));
            Assert.AreEqual("", s3);
            Assert.AreEqual(PiffReadStatuses.EofPremature, sut.ReadAsciiString(2, out _));
        }
    }
}
