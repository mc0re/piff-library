using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;


namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class FtypTests
    {
        [TestMethod]
        public void Ftype_ReadNoCompat()
        {
            var bytes = new byte[] { 0, 0, 0, 16, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x61, 0x6A, 0x72, 0, 0, 0, 2 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(16L, length);
            var ftyp = box as PiffFileTypeBox;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual("majr", ftyp.MajorBrand);
            Assert.AreEqual(2u, ftyp.MinorVersion);
            Assert.AreEqual(0, ftyp.CompatibleBrands.Length);
        }


        [TestMethod]
        public void Ftype_ReadTwoCompat()
        {
            var bytes = new byte[] {
                0, 0, 0, 24, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x61, 0x6A, 0x72, 0, 0, 0, 2,
                0x63, 0x6F, 0x6D, 0x31, 0x63, 0x6F, 0x6D, 0x32 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(24L, length);
            var ftyp = box as PiffFileTypeBox;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual("majr", ftyp.MajorBrand);
            Assert.AreEqual(2u, ftyp.MinorVersion);
            Assert.AreEqual(2, ftyp.CompatibleBrands.Length);
            Assert.AreEqual("com1", ftyp.CompatibleBrands[0]);
            Assert.AreEqual("com2", ftyp.CompatibleBrands[1]);
        }


        [TestMethod]
        public void Ftype_ReadNoCompat64bit()
        {
            var bytes = new byte[] {
                0, 0, 0, 1, 0x66, 0x74, 0x79, 0x70, 0, 0, 0, 0, 0, 0, 0, 24, 0x6D, 0x61, 0x6A, 0x72, 0, 0, 0, 2 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(24L, length);
            var ftyp = box as PiffFileTypeBox;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual("majr", ftyp.MajorBrand);
            Assert.AreEqual(2u, ftyp.MinorVersion);
            Assert.AreEqual(0, ftyp.CompatibleBrands.Length);
        }


        [TestMethod]
        public void Ftype_ReadLimitedCompat()
        {
            // Two items in the last array, but the box length limits to 1
            var bytes = new byte[] {
                0, 0, 0, 20, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x61, 0x6A, 0x72, 0, 0, 0, 2,
                0x63, 0x6F, 0x6D, 0x31, 0x63, 0x6F, 0x6D, 0x32 };
            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(20L, length);
            var ftyp = box as PiffFileTypeBox;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual("majr", ftyp.MajorBrand);
            Assert.AreEqual(2u, ftyp.MinorVersion);
            Assert.AreEqual(1, ftyp.CompatibleBrands.Length);
            Assert.AreEqual("com1", ftyp.CompatibleBrands[0]);
        }


        [TestMethod]
        public void Ftype_LengthTwoCompat()
        {
            var box = new PiffFileTypeBox()
            {
                MajorBrand = "majr",
                MinorVersion = 2,
                CompatibleBrands = new [] { "com1", "com2" }
            };

            var written = PiffWriter.GetBoxLength(box);

            Assert.IsNotNull(written);
            Assert.AreEqual(24uL, written);
        }


        [TestMethod]
        public void Ftype_WriteTwoCompat()
        {
            using var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var ctx = new PiffWriteContext();

            var box = new PiffFileTypeBox()
            {
                MajorBrand = "majr",
                MinorVersion = 2,
                CompatibleBrands = new [] { "com1", "com2" }
            };
            var bytes = new byte[] {
                0, 0, 0, 24, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x61, 0x6A, 0x72, 0, 0, 0, 2,
                0x63, 0x6F, 0x6D, 0x31, 0x63, 0x6F, 0x6D, 0x32 };

            PiffWriter.WriteBox(output, box, ctx);

            var written = ms.GetBuffer().Take((int)ms.Length).ToArray();
            Assert.IsNotNull(written);
            Assert.AreEqual(24, written.Length);
            TestUtil.Compare(bytes, written);
        }
    }
}
