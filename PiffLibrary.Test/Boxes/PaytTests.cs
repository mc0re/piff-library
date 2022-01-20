using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;


namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class PaytTests
    {
        private static readonly byte[] PaytSample =
        {
            0, 0, 0, 19, 0x70, 0x61, 0x79, 0x74,
            0, 0, 0, 1,
            6, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66
        };


        [TestMethod]
        public void Payt_Read()
        {
            using var input = new BitReadStream(new MemoryStream(PaytSample, false), true);
            var ctx = new PiffReadContext { AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(19L, length);
            var payt = box as PiffHintPayloadIdBox;
            Assert.IsNotNull(payt);
            Assert.AreEqual(1u, payt.PayloadId);
            Assert.AreEqual(6, payt.Count);
            Assert.AreEqual("abcdef", payt.RtpMap);
        }
    }
}
