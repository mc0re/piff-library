using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;

namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class EsdsTests
    {
        private byte[] EsdsSample = new byte[]
        {
            0, 0, 0, 39, 0x65, 0x73, 0x64, 0x73, 0, 0, 0, 0,
            3, 25, 0x00, 0x00, 0x00,
            4, 17, 0x40, 0x15, 0, 0, 0, 0x00, 0x02, 0xA6, 0x6D, 0x00, 0x02, 0xA6, 0x6D,
            5, 2, 0x11, 0x90,
            6, 1, 0x02
        };


        [TestMethod]
        public void Esds_Read()
        {
            using var input = new BitReadStream(new MemoryStream(EsdsSample, false), true);
            var ctx = new PiffReadContext { AnyRoot = true };

            var status = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(PiffReadStatuses.Continue, status);
            var esds = box as PiffElementaryStreamDescriptionBox;
            Assert.IsNotNull(esds);
            Assert.AreEqual(PiffEsdsBlockIds.Esd, esds.Descriptor.Tag);
            Assert.AreEqual(25, esds.Descriptor.Length);
        }


        [TestMethod]
        public void Esds_Write()
        {
            var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var ctx = new PiffWriteContext();

            var box = new PiffElementaryStreamDescriptionBox
            {
                Descriptor = new PiffDescriptorContainer
                {
                    Tag = PiffEsdsBlockIds.Esd,
                    Esd = new PiffElementaryStreamDescriptor
                    {
                    },
                    Children = new[]
                    {
                        new PiffDescriptorContainer
                        {
                            Tag = PiffEsdsBlockIds.Dcd,
                            Dcd = new PiffDecoderConfigDescription
                            {
                            }
                        }
                    }
                }
            };
            PiffWriter.WriteBox(output, box, ctx);
            
            CollectionAssert.AreEqual(EsdsSample, ms.GetBuffer().Take((int) ms.Length).ToArray());
        }
    }
}
