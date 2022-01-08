using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var bytes = new byte[] { 0, 0, 0, 16, 0X66, 0X74, 0X79, 0X70, 0X6D, 0X61, 0X6A, 0X72, 0, 0, 0, 2 };
            using var ms = new MemoryStream(bytes, false);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(ms, ctx, out var box);

            Assert.IsNotNull(box);
            Assert.AreEqual(16uL, length);
            var ftyp = box as PiffFileType;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual("majr", ftyp.MajorBrand);
            Assert.AreEqual(2u, ftyp.MinorVersion);
            Assert.AreEqual(0, ftyp.CompatibleBrands.Length);
        }


        [TestMethod]
        public void Ftype_ReadTwoCompat()
        {
            var bytes = new byte[] {
                0, 0, 0, 24, 0X66, 0X74, 0X79, 0X70, 0X6D, 0X61, 0X6A, 0X72, 0, 0, 0, 2,
                0X63, 0X6F, 0X6D, 0X31, 0X63, 0X6F, 0X6D, 0X32 };
            using var ms = new MemoryStream(bytes, false);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(ms, ctx, out var box);

            Assert.IsNotNull(box);
            Assert.AreEqual(24uL, length);
            var ftyp = box as PiffFileType;
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
                0, 0, 0, 1, 0X66, 0X74, 0X79, 0X70, 0, 0, 0, 0, 0, 0, 0, 24, 0X6D, 0X61, 0X6A, 0X72, 0, 0, 0, 2 };
            using var ms = new MemoryStream(bytes, false);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(ms, ctx, out var box);

            Assert.IsNotNull(box);
            Assert.AreEqual(24uL, length);
            var ftyp = box as PiffFileType;
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
                0, 0, 0, 20, 0X66, 0X74, 0X79, 0X70, 0X6D, 0X61, 0X6A, 0X72, 0, 0, 0, 2,
                0X63, 0X6F, 0X6D, 0X31, 0X63, 0X6F, 0X6D, 0X32 };
            using var ms = new MemoryStream(bytes, false);
            var ctx = new PiffReadContext();

            var length = PiffReader.ReadBox(ms, ctx, out var box);

            Assert.IsNotNull(box);
            Assert.AreEqual(20uL, length);
            var ftyp = box as PiffFileType;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual("majr", ftyp.MajorBrand);
            Assert.AreEqual(2u, ftyp.MinorVersion);
            Assert.AreEqual(1, ftyp.CompatibleBrands.Length);
            Assert.AreEqual("com1", ftyp.CompatibleBrands[0]);
        }


        [TestMethod]
        public void Ftype_LengthTwoCompat()
        {
            var box = new PiffFileType()
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
            var ctx = new PiffWriteContext();

            var box = new PiffFileType()
            {
                MajorBrand = "majr",
                MinorVersion = 2,
                CompatibleBrands = new [] { "com1", "com2" }
            };
            var bytes = new byte[] {
                0, 0, 0, 24, 0X66, 0X74, 0X79, 0X70, 0X6D, 0X61, 0X6A, 0X72, 0, 0, 0, 2,
                0X63, 0X6F, 0X6D, 0X31, 0X63, 0X6F, 0X6D, 0X32 };

            PiffWriter.WriteBox(ms, box, ctx);

            var written = ms.GetBuffer().Take((int)ms.Length).ToArray();
            Assert.IsNotNull(written);
            Assert.AreEqual(24, written.Length);
            CollectionAssert.AreEqual(bytes, written);
        }
    }
}
