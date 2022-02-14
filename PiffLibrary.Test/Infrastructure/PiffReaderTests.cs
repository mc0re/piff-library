using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace PiffLibrary.Test
{
    [TestClass]
    public class PiffReaderTests
    {
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

            Assert.AreEqual(6103, stream.Length);
            var actual = stream.GetBuffer().Take((int) stream.Length).ToArray();
            var expected = File.ReadAllBytes("Data/piff-result.bin");
            TestUtil.Compare(expected.Take((int) stream.Length).ToArray(), actual);

            // Read it back out
            stream.Position = 0;
            var piff = PiffFile.ParseButSkipData(stream);

            Assert.IsNotNull(piff);
        }
    }
}