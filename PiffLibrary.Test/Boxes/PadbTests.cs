using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;


namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class PadbTests
    {
        [TestMethod]
        public void Padb_ReadNoSamples()
        {
            var bytes = new byte[] { 0, 0, 0, 16, 0x70, 0x61, 0x64, 0x62, 0, 0, 0, 0, 0, 0, 0, 0 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext{ AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(16L, length);
            var padb = box as PiffPaddingBitsBox;
            Assert.IsNotNull(padb);
            Assert.AreEqual(0u, padb.SampleCount);
            Assert.IsNull(padb.Padding);
        }


        [TestMethod]
        public void Padb_ReadTwoSamples()
        {
            var bytes = new byte[] { 0, 0, 0, 17, 0x70, 0x61, 0x64, 0x62, 0, 0, 0, 0, 0, 0, 0, 2, 0x77 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext{ AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(17L, length);
            var padb = box as PiffPaddingBitsBox;
            Assert.IsNotNull(padb);
            Assert.AreEqual(2u, padb.SampleCount);
            Assert.AreEqual(1, padb.Padding.Length);
            Assert.AreEqual(0x77, padb.Padding[0]);
        }


        [TestMethod]
        public void Padb_ReadThreeSamples()
        {
            var bytes = new byte[] { 0, 0, 0, 18, 0x70, 0x61, 0x64, 0x62, 0, 0, 0, 0, 0, 0, 0, 3, 0x77, 0x50 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext{ AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(18L, length);
            var padb = box as PiffPaddingBitsBox;
            Assert.IsNotNull(padb);
            Assert.AreEqual(3u, padb.SampleCount);
            Assert.AreEqual(2, padb.Padding.Length);
            Assert.AreEqual(0x77, padb.Padding[0]);
            Assert.AreEqual(0x50, padb.Padding[1]);
        }


        [TestMethod]
        public void Padb_ReadBoxTooLong()
        {
            var bytes = new byte[] { 0, 0, 0, 18, 0x70, 0x61, 0x64, 0x62, 0, 0, 0, 0, 0, 0, 0, 2, 0x77, 0 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext{ AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(1, ctx.Messages.Count);
            Assert.IsNotNull(box);
            // It tries to read "PiffBoxBase.Children" and cannot read the length
            Assert.AreEqual(PiffReader.EofPremature, length);
            var padb = box as PiffPaddingBitsBox;
            Assert.IsNotNull(padb);
            Assert.AreEqual(2u, padb.SampleCount);
            Assert.AreEqual(1, padb.Padding.Length);
            Assert.AreEqual(0x77, padb.Padding[0]);
        }


        [TestMethod]
        public void Padb_LengthThreeSamples()
        {
            var box = new PiffPaddingBitsBox
            {
                SampleCount = 3,
                Padding = new byte[] { 0x77, 0x50 }
            };

            var written = PiffWriter.GetBoxLength(box);

            Assert.IsNotNull(written);
            Assert.AreEqual(18uL, written);
        }


        [TestMethod]
        public void Padb_WriteThreeSamples()
        {
            using var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var ctx = new PiffWriteContext();

            var box = new PiffPaddingBitsBox
            {
                SampleCount = 3,
                Padding = new byte[] { 0x77, 0x50 }
            };
            var bytes = new byte[] { 0, 0, 0, 18, 0x70, 0x61, 0x64, 0x62, 0, 0, 0, 0, 0, 0, 0, 3, 0x77, 0x50 };

            PiffWriter.WriteBox(output, box, ctx);

            var written = ms.GetBuffer().Take((int)ms.Length).ToArray();
            Assert.IsNotNull(written);
            Assert.AreEqual(18, written.Length);
            TestUtil.Compare(bytes, written);
        }
    }
}
