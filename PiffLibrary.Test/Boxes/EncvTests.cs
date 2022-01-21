using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;


namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class EncvTests
    {
        private static readonly byte[] EncvSample =
        {
            0, 0, 0, 234, 0x65, 0x6E, 0x63, 0x76, // Lvl 1: encv box
            0, 0, 0, 0, 0, 0, // reserved
            0, 1, // DataReferenceIndex
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // reserved
            0x07, 0x80, // width
            0x04, 0x38, // height
            0x00, 0x48, 0x00, 0x00, 0x00, 0x48, 0x00, 0x00, // H and V resolutions
            0, 0, 0, 0,
            0, 1, // Frame count
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // compressor name
            0x00, 0x18, // depth
            0xFF, 0xFF,
            0, 0, 0, 48, 0x61, 0x76, 0x63, 0x43, // 2: avcC box
            1, 0x42, 0xC0, 0x28, 0xFF, 0xE1, 0x00, 0x19,
            0x67, 0x42, 0xC0, 0x28, 0xD9, 0x00, 0x78, 0x02,
            0x27, 0xE5, 0x84, 0x00, 0x00, 0x03, 0x00, 0x04,
            0x00, 0x00, 0x03, 0x00, 0xF0, 0x3C, 0x60, 0xC9,
            0x20, 0x01, 0x00, 0x04, 0x68, 0xCB, 0x8C, 0xB2,
            0, 0, 0, 20, 0x62, 0x74, 0x72, 0x74, // 2: brtr box
            0, 0, 0, 0, // BufferSize
            0x00, 0x03, 0x0A, 0xEF, //  MaxBitrate
            0x00, 0x03, 0x0A, 0xEF, // AverageBitrate
            0, 0, 0, 80, 0x73, 0x69, 0x6E, 0x66, // 2: sinf box
            0, 0, 0, 12, 0x66, 0x72, 0x6D, 0x61, // 3: frma box
            0x61, 0x76, 0x63, 0x31, // original format = "avc1"
            0, 0, 0, 20, 0x73, 0x63, 0x68, 0x6D, // 3: schm box
            0, 0, 0, 0, // Flags: no SchemeUrl
            0x63, 0x65, 0x6E, 0x63, // SchemeType = "cenc"
            0, 1, 0, 0, // SchemeVersion
            0, 0, 0, 40, 0x73, 0x63, 0x68, 0x69, // 3: schi box
            0, 0, 0, 32, 0x74, 0x65, 0x6E, 0x63, // 4: tenc box
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x08,
            0xCD, 0x5D, 0xE0, 0x88, 0xBD, 0x2C, 0x42, 0xB8,
            0xA2, 0xED, 0x98, 0xB8, 0x65, 0x32, 0x48, 0xE8
        };


        [TestMethod]
        public void Encv_Read()
        {
            using var input = new BitReadStream(new MemoryStream(EncvSample, false), true);
            var ctx = new PiffReadContext { AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(234uL, length);
            var encv = box as PiffVideoSampleEntryBox;
            Assert.IsNotNull(encv);
        }
    }
}
