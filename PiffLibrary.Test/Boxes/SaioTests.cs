using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;


namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class SaioTests
    {
        private byte[] SaioSample = new byte[]
        {
           0, 0, 0, 20, 0x73, 0x61, 0x69, 0x6F,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x09, 0x4F, 
        };


        [TestMethod]
        public void Saio_Read()
        {
            using var input = new BitReadStream(new MemoryStream(SaioSample, false), true);
            var ctx = new PiffReadContext { AnyRoot = true };

            var status = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(PiffReadStatuses.Continue, status);
            var saio = box as PiffSampleAuxiliaryOffsetBox;
            Assert.IsNotNull(saio);
            Assert.AreEqual(1u, saio.Count);
            Assert.AreEqual(0x94Fu, saio.Offset[0]);
        }


        [TestMethod]
        public void Saio_Write()
        {
            var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var ctx = new PiffWriteContext();

            var box = new PiffSampleAuxiliaryOffsetBox
            {
                Count = 1,
                Offset = new ulong[] { 0x94F }
            };
            PiffWriter.WriteBox(output, box, ctx);
            
            TestUtil.Compare(SaioSample, ms.GetBuffer().Take((int) ms.Length).ToArray());
        }
    }
}
