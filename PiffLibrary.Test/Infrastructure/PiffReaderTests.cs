using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;


namespace PiffLibrary.Test
{
    [TestClass]
    public class PiffReaderTests
    {
        [TestMethod]
        public void Reader_ReadSignedIntsFull()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestSignedInts();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.Continue, status);
            Assert.AreEqual(0x01, obj.Byte);
            Assert.AreEqual(0x0203, obj.Short);
            Assert.AreEqual(0x04050607, obj.Int);
            Assert.AreEqual(0x08090a0b0c0d0e0f, obj.Long);
        }


        [TestMethod]
        public void Reader_ReadSignedIntsProp()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestSignedInts();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.Continue, status);
            Assert.AreEqual(0x01, obj.Byte);
            Assert.AreEqual(0x0203, obj.Short);
            Assert.AreEqual(0x04050607, obj.Int);
            Assert.AreEqual(0L, obj.Long);
        }


        [TestMethod]
        public void Reader_ReadSignedIntsCut()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestSignedInts();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.EofPremature, status);
            Assert.AreEqual(0x01, obj.Byte);
            Assert.AreEqual(0x0203, obj.Short);
            Assert.AreEqual(0, obj.Int);
            Assert.AreEqual(0L, obj.Long);
        }


        [TestMethod]
        public void Reader_ReadUnsignedIntsFull()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestUnsignedInts();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.Continue, status);
            Assert.AreEqual(0x01, obj.Byte);
            Assert.AreEqual(0x0203, obj.Short);
            Assert.AreEqual(0x04050607u, obj.Int);
            Assert.AreEqual(0x08090a0b0c0d0e0fuL, obj.Long);
        }


        [TestMethod]
        public void Reader_ReadIntArrayFull()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 2, 0x41, 0x62, 0, 0, 0, 1, 0, 0, 0, 2 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestIntArray();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.Continue, status);
            Assert.AreEqual("Ab", obj.Name);
            Assert.AreEqual(2, obj.Numbers.Length);
            Assert.AreEqual(1, obj.Numbers[0]);
            Assert.AreEqual(2, obj.Numbers[1]);
        }


        [TestMethod]
        public void Reader_ReadIntArrayCut()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 2, 0x41, 0x62, 0, 0, 0, 1, 0, 0, 0 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestIntArray();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.EofPremature, status);
            Assert.AreEqual("Ab", obj.Name);
            Assert.AreEqual(1, obj.Numbers.Length);
            Assert.AreEqual(1, obj.Numbers[0]);
        }


        [TestMethod]
        public void Reader_ReadObjectArrayFull()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 0x41, 0x62, 0, 0, 0, 0, 1, 0, 0, 0, 2 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestObjArray();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.Continue, status);
            Assert.AreEqual("Ab", obj.Name);
            Assert.AreEqual(2, obj.Items.Length);
            Assert.AreEqual(1, obj.Items[0].Number);
            Assert.AreEqual(2, obj.Items[1].Number);
        }


        [TestMethod]
        public void Reader_ReadObjectArrayCut()
        {
            using var input = new BitReadStream(new MemoryStream(new byte[] { 0x41, 0x62, 0, 0, 0, 0, 1, 0, 0, 0 }), true);
            var ctx = new PiffReadContext();
            var obj = new TestObjArray();

            var status = PiffPropertyInfo.ReadObject(obj, input, ctx);

            Assert.AreEqual(PiffReadStatuses.EofPremature, status);
            Assert.AreEqual("Ab", obj.Name);
            Assert.AreEqual(1, obj.Items.Length);
            Assert.AreEqual(1, obj.Items[0].Number);
        }


        [TestMethod]
        public void Reader_MinimalRoundtrip()
        {
            var ctx = new PiffWriteContext();
            using var stream = new MemoryStream();

            // Write the data
            PiffWriter.WriteHeader(stream, PiffWriterTests.SpeedwayManifest, ctx);

            // Data chunks are MOOF boxes
            var audioChunk = File.ReadAllBytes("Data/moof-1.bin");
            var audioOffset = stream.Position;
            stream.Write(audioChunk, 0, audioChunk.Length);

            var audioData = File.ReadAllBytes("Data/mdat.bin");
            stream.Write(audioData, 0, audioData.Length);

            var videoChunk = File.ReadAllBytes("Data/moof-2.bin");
            var videoOffset = stream.Position;
            stream.Write(videoChunk, 0, videoChunk.Length);

            var videoData = File.ReadAllBytes("Data/mdat.bin");
            stream.Write(videoData, 0, videoData.Length);

            var audioOffsets = new[] { new PiffSampleOffsetDto { Time = 0, Offset = (ulong) audioOffset } };
            var videoOffsets = new[] { new PiffSampleOffsetDto { Time = 0, Offset = (ulong) videoOffset } };
            PiffWriter.WriteFooter(stream, PiffWriterTests.SpeedwayManifest, audioOffsets, videoOffsets, ctx);

            // Dump for HEX viewer
            stream.Position = 0;
            using (var dump = File.OpenWrite(@"C:\Temp\piff.bin"))
                stream.CopyTo(dump);

            Assert.AreEqual(6112, stream.Length);

            // Read it back out
            stream.Position = 0;
            var piff = PiffFile.ParseButSkipData(stream);

            Assert.IsNotNull(piff);
        }
    }
}