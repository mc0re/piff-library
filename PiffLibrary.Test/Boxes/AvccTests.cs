using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System.IO;
using System.Linq;

namespace PiffLibrary.Test.Boxes
{
    [TestClass]
    public class AvccTests
    {
        private byte[] AvccSample = new byte[]
        {
            0, 0, 0, 77, 0x61, 0x76, 0x63, 0x43,
            1, 0x64, 0, 13,
            0xFF, 0xE1, 0, 53,
            0x67, 0x64, 0x00, 0x0D, 0xAC, 0x2C, 0xA5, 0x0E, 0x11, 0xBF,
            0xF0, 0x40, 0x00, 0x3F, 0x05, 0x20, 0xC0, 0xC0, 0xC8, 0x00,
            0x00, 0x1F, 0x48, 0x00, 0x05, 0xDC, 0x03, 0x00, 0x00, 0x1C,
            0x12, 0x00, 0x03, 0x82, 0x73, 0xF8, 0xC7, 0x18, 0x00, 0x00,
            0xE0, 0x90, 0x00, 0x1C, 0x13, 0x9F, 0xC6, 0x38, 0x76, 0x84,
            0x89, 0x45, 0x80,
            1, 0, 5,
            0x68, 0xE9, 0x09, 0x35, 0x25
        };


        [TestMethod]
        public void Avcc_Read()
        {
            using var input = new BitReadStream(new MemoryStream(AvccSample, false), true);
            var ctx = new PiffReadContext{ AnyRoot = true };

            var length = PiffReader.ReadBox(input, ctx, out var box);

            Assert.AreEqual(0, ctx.Messages.Count, ctx.Messages.Any() ? ctx.Messages.First() : "");
            Assert.IsNotNull(box);
            Assert.AreEqual(77L, length);
            var ftyp = box as PiffAvcConfigurationBox;
            Assert.IsNotNull(ftyp);
            Assert.AreEqual<byte>(1, ftyp.ConfigurationVersion);
            Assert.AreEqual<byte>(0x64, ftyp.AvcProfile);
            Assert.AreEqual<byte>(13, ftyp.AvcLevel);
            Assert.AreEqual<byte>(1, ftyp.SequenceSlotCount);
            Assert.AreEqual<short>(53, ftyp.SequenceSlotLength);
            Assert.AreEqual<byte>(1, ftyp.PictureSlotCount);
            Assert.AreEqual<short>(5, ftyp.PictureSlotLength);
        }


        [TestMethod]
        public void Avcc_Length()
        {
            var box = new PiffAvcConfigurationBox()
            {
                ConfigurationVersion = 1,
                AvcProfile = 0x64,
                AvcLevel = 13,
                SequenceSlotCount = 1,
                SequenceSlotLength = 53,
                SequenceSlotData = new byte[] {
                    0x67, 0x64, 0x00, 0x0D, 0xAC, 0x2C, 0xA5, 0x0E, 0x11, 0xBF,
                    0xF0, 0x40, 0x00, 0x3F, 0x05, 0x20, 0xC0, 0xC0, 0xC8, 0x00,
                    0x00, 0x1F, 0x48, 0x00, 0x05, 0xDC, 0x03, 0x00, 0x00, 0x1C,
                    0x12, 0x00, 0x03, 0x82, 0x73, 0xF8, 0xC7, 0x18, 0x00, 0x00,
                    0xE0, 0x90, 0x00, 0x1C, 0x13, 0x9F, 0xC6, 0x38, 0x76, 0x84,
                    0x89, 0x45, 0x80 },
                PictureSlotCount = 1,
                PictureSlotLength = 5,
                PictureSlotData = new byte[] { 0x68, 0xE9, 0x09, 0x35, 0x25 }
            };

            var length = PiffWriter.GetBoxLength(box);
            Assert.AreEqual(77uL, length);
        }


        [TestMethod]
        public void Avcc_Write()
        {
            var box = new PiffAvcConfigurationBox()
            {
                ConfigurationVersion = 1,
                AvcProfile = 0x64,
                AvcLevel = 13,
                SequenceSlotCount = 1,
                SequenceSlotLength = 53,
                SequenceSlotData = new byte[] {
                    0x67, 0x64, 0x00, 0x0D, 0xAC, 0x2C, 0xA5, 0x0E, 0x11, 0xBF,
                    0xF0, 0x40, 0x00, 0x3F, 0x05, 0x20, 0xC0, 0xC0, 0xC8, 0x00,
                    0x00, 0x1F, 0x48, 0x00, 0x05, 0xDC, 0x03, 0x00, 0x00, 0x1C,
                    0x12, 0x00, 0x03, 0x82, 0x73, 0xF8, 0xC7, 0x18, 0x00, 0x00,
                    0xE0, 0x90, 0x00, 0x1C, 0x13, 0x9F, 0xC6, 0x38, 0x76, 0x84,
                    0x89, 0x45, 0x80 },
                PictureSlotCount = 1,
                PictureSlotLength = 5,
                PictureSlotData = new byte[] { 0x68, 0xE9, 0x09, 0x35, 0x25 }
            };

            var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var ctx = new PiffWriteContext();

            PiffWriter.WriteBox(output, box, ctx);

            var written = ms.GetBuffer().Take((int) ms.Length).ToArray();
            Assert.IsNotNull(written);
            Assert.AreEqual(77, written.Length);
            TestUtil.Compare(AvccSample, written);
        }
    }
}
