using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;


namespace PiffLibrary.Test
{
    [TestClass]
    public class PiffReaderTests
    {
        [TestMethod]
        public void Reader_MinimalRoundtrip()
        {
            using var stream = new MemoryStream();

            // Write the data
            PiffWriter.WriteHeader(stream, PiffWriterTests.SpeedwayManifest);

            // Data chunks are MOOF boxes
            var audioChunk = File.ReadAllBytes("Data/moof.bin");
            var audioOffset = stream.Position;
            stream.Write(audioChunk, 0, audioChunk.Length);

            var videoChunk = new byte[] { 2, 2, 2, 2 };
            var videoOffset = stream.Position;
            stream.Write(videoChunk, 0, videoChunk.Length);

            var audioOffsets = new[] { new PiffSampleOffset { Time = 0, Offset = audioOffset } };
            var videoOffsets = new[] { new PiffSampleOffset { Time = 0, Offset = videoOffset } };
            PiffWriter.WriteFooter(stream, PiffWriterTests.SpeedwayManifest, audioOffsets, videoOffsets);

            Assert.AreEqual(4092, stream.Length);

            // Dump for HEX viewer
            stream.Position = 0;
            using (var dump = File.OpenWrite(@"C:\Temp\piff.bin"))
                stream.CopyTo(dump);

            // Read it back out
            stream.Position = 0;
            var piff = PiffFile.Parse(stream);

            Assert.IsNotNull(piff);
        }
    }
}