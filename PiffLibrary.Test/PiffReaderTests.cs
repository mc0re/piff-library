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
            var piffManifest = new PiffManifest
            {
                Created = new DateTime(2022, 2, 1),
                TotalDuration = TimeSpan.FromSeconds(1),
                TimeScale = 1000,
                AudioTrackId = 1,
                VideoTrackId = 2,
                KeyIdentifier = Guid.NewGuid(),
                Audio = new PiffAudioManifest
                {
                    Duration = 1000,
                    BitRate = 44100,
                    Channels = 2,
                    CodecId = "AACL",
                    CodecData = new byte[] { 0, 0 }
                },
                Video = new PiffVideoManifest
                {
                    Duration = 1000,
                    BitRate = 6000,
                    Width = 128,
                    Height = 96,
                    CodecId = "H264",
                    // See PiffAvcConfiguration
                    CodecData = new byte[] { 0, 0, 0, 1, 0x67, 0x42, 1, 1, 0, 0, 0, 1, 0x68 }
                },
                ProtectionSystemId = Guid.NewGuid(),
                ProtectionData = new byte[] { 3, 3, 3, 3 }
            };

            PiffWriter.WriteHeader(stream, piffManifest);

            var audioChunk = new byte[] { 1, 1, 1, 1 };
            var audioOffset = stream.Position;
            stream.Write(audioChunk, 0, audioChunk.Length);

            var videoChunk = new byte[] { 2, 2, 2, 2 };
            var videoOffset = stream.Position;
            stream.Write(videoChunk, 0, videoChunk.Length);

            var audioOffsets = new[] { new PiffSampleOffset { Time = 0, Offset = audioOffset } };
            var videoOffsets = new[] { new PiffSampleOffset { Time = 0, Offset = videoOffset } };
            PiffWriter.WriteFooter(stream, piffManifest, audioOffsets, videoOffsets);

            Assert.AreEqual(1544, stream.Length);

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