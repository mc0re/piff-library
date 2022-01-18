using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;


namespace PiffLibrary.Test.Boxes
{
[TestClass]
    public class TfraTests
    {
        [TestMethod]
        public void Tfra_Read()
        {
            var bytes = new byte[]
            {
                0, 0, 0, 43, 0x74, 0x66, 0x72, 0x61, 1, 0, 0, 0,
                0, 0, 0, 1,
                0, 0, 0, 0,
                0, 0, 0, 1,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x09, 0x4F, 1, 1, 1
            };

            using var input = new BitReadStream(new MemoryStream(bytes, false), true);
            var ctx = new PiffReadContext { AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(43L, length);
            var tfra = box as PiffTrackFragmentRandomAccessBox;
            Assert.IsNotNull(tfra);
            Assert.AreEqual(1u, tfra.Count);
        }
    }
}
