using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
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
            var audioOffset = (ulong) stream.Position;
            stream.Write(audioChunk, 0, audioChunk.Length);

            var audioData = File.ReadAllBytes("Data/mdat.bin");
            stream.Write(audioData, 0, audioData.Length);

            var videoChunk = File.ReadAllBytes("Data/moof-2.bin");
            var videoOffset = (ulong) stream.Position;
            stream.Write(videoChunk, 0, videoChunk.Length);

            var videoData = File.ReadAllBytes("Data/mdat.bin");
            stream.Write(videoData, 0, videoData.Length);

            var audioOffsets = new[] { new PiffSampleOffsetDto { Time = 0, Offset = audioOffset } };
            var videoOffsets = new[] { new PiffSampleOffsetDto { Time = 0, Offset = videoOffset } };
            var endOffset = (ulong) stream.Position;
            PiffWriter.WriteFooter(stream, PiffWriterTests.SpeedwayManifest, audioOffsets, videoOffsets, ctx);

            Assert.AreEqual(6103, stream.Length);
            var actual = stream.GetBuffer().Take((int) stream.Length).ToArray();
            var expected = File.ReadAllBytes("Data/piff-result.bin");
            TestUtil.Compare(expected.Take((int) stream.Length).ToArray(), actual);

            // Read it back out
            stream.Position = 0;
            var rctx = new PiffReadContext();
            var piff = PiffFile.Parse(stream, rctx);

            Assert.IsNotNull(piff);

            Assert.AreEqual(7, piff.Boxes.Count);
            Assert.AreEqual(0uL, piff.GetSingleBox<PiffFileTypeBox>().OriginalPosition);
            Assert.AreEqual(24uL, piff.GetSingleBox<PiffMovieBox>().OriginalPosition);
            var moofs = piff.Boxes.OfType<PiffMovieFragmentBox>().ToArray();
            Assert.AreEqual(2, moofs.Length);
            Assert.AreEqual(audioOffset, moofs[0].OriginalPosition);
            Assert.AreEqual(videoOffset, moofs[1].OriginalPosition);
            Assert.AreEqual(endOffset, piff.GetSingleBox<PiffMovieFragmentRandomAccessBox>().OriginalPosition);
        }
    }
}
