using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PiffLibrary.Test
{
    [TestClass]
    public class PiffWriterTests
    {
        private static readonly Guid PlayReadyGuid = Guid.Parse("{9a04f079-9840-4286-ab92-e65be0885f95}");

        private static readonly byte[] PlayReadyProtectionData =
            Convert.FromBase64String(@"jAMAAAEAAQCCAzwAVwBSAE0ASABFAEEARABFAFIAIAB4AG0AbABuAHMAPQAiAGgAdAB0AHAAOgAvAC8AcwBjAGgAZQBtAGEAcwAuAG0AaQBjAHIAbwBzAG8AZgB0AC4AYwBvAG0ALwBEAFIATQAvADIAMAAwADcALwAwADMALwBQAGwAYQB5AFIAZQBhAGQAeQBIAGUAYQBkAGUAcgAiACAAdgBlAHIAcwBpAG8AbgA9ACIANAAuADAALgAwAC4AMAAiAD4APABEAEEAVABBAD4APABQAFIATwBUAEUAQwBUAEkATgBGAE8APgA8AEsARQBZAEwARQBOAD4AMQA2ADwALwBLAEUAWQBMAEUATgA+ADwAQQBMAEcASQBEAD4AQQBFAFMAQwBUAFIAPAAvAEEATABHAEkARAA+ADwALwBQAFIATwBUAEUAQwBUAEkATgBGAE8APgA8AEsASQBEAD4AQQBtAGYAagBDAFQATwBQAGIARQBPAGwAMwBXAEQALwA1AG0AYwBlAGMAQQA9AD0APAAvAEsASQBEAD4APABDAEgARQBDAEsAUwBVAE0APgBCAEcAdwAxAGEAWQBaADEAWQBYAE0APQA8AC8AQwBIAEUAQwBLAFMAVQBNAD4APABDAFUAUwBUAE8ATQBBAFQAVABSAEkAQgBVAFQARQBTAD4APABJAEkAUwBfAEQAUgBNAF8AVgBFAFIAUwBJAE8ATgA+ADcALgAxAC4AMQAwADYANAAuADAAPAAvAEkASQBTAF8ARABSAE0AXwBWAEUAUgBTAEkATwBOAD4APAAvAEMAVQBTAFQATwBNAEEAVABUAFIASQBCAFUAVABFAFMAPgA8AEwAQQBfAFUAUgBMAD4AaAB0AHQAcAA6AC8ALwBwAGwAYQB5AHIAZQBhAGQAeQAuAGQAaQByAGUAYwB0AHQAYQBwAHMALgBuAGUAdAAvAHAAcgAvAHMAdgBjAC8AcgBpAGcAaAB0AHMAbQBhAG4AYQBnAGUAcgAuAGEAcwBtAHgAPAAvAEwAQQBfAFUAUgBMAD4APABEAFMAXwBJAEQAPgBBAEgAKwAwADMAagB1AEsAYgBVAEcAYgBIAGwAMQBWAC8AUQBJAHcAUgBBAD0APQA8AC8ARABTAF8ASQBEAD4APAAvAEQAQQBUAEEAPgA8AC8AVwBSAE0ASABFAEEARABFAFIAPgA=");

        internal static readonly PiffManifest SpeedwayManifest = new PiffManifest
            {
                // Time gives 0xC98FDA78 = 3_381_647_992 seconds
                Created = new DateTime(2011, 2, 27, 10, 39, 52),
                TotalDuration = TimeSpan.FromSeconds(1),
                TimeScale = 10_000_000,
                AudioTrackId = 1,
                VideoTrackId = 2,
                KeyIdentifier = Guid.Parse("{09e36702-8f33-436c-a5dd-60ffe6671e70}"),
                Audio = new PiffAudioManifest
                {
                    BitRate = 128000,
                    BitsPerSample = 16,
                    Channels = 2,
                    CodecId = "AACL",
                    CodecData = new byte[] { 18, 16 },
                    Duration = 1209527438,
                    SamplingRate = 44100
                },
                Video = new PiffVideoManifest
                {
                    BitRate = 230000,
                    Duration = 1207870001,
                    Width = 224,
                    Height = 128,
                    CodecId = "H264",
                    // See PiffAvcConfiguration
                    CodecData = new byte[] {
                        0, 0, 0, 1, 0x67, 100, 0, 13, 172,
                        44, 165, 14, 17, 191, 240, 64, 0,
                        63, 5, 32, 192, 192, 200, 0,
                        0, 31, 72, 0, 5, 220, 3, 0,
                        0, 28, 18, 0, 3, 130, 115, 248,
                        199, 24, 0, 0, 224, 144, 0, 28,
                        19, 159, 198, 56, 118, 132, 137, 69,
                        128, 0, 0, 0, 1, 104, 233, 9,
                        53, 37 }
                },
                ProtectionSystemId = PlayReadyGuid,
                ProtectionData = PlayReadyProtectionData
            };


        [TestMethod]
        public void Writer_WriteBox()
        {
            var ftyp = new byte[] {
                0, 0, 0, 20, 0x66, 0x74, 0x79, 0x70, 0x61, 0x62, 0x63, 0x64, 0, 0, 0, 10,
                0x65, 0x66, 0x67, 0x68 };

            var ctx = new PiffWriteContext();
            var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var box = new PiffFileTypeBox { MajorBrand = "abcd", MinorVersion = 10, CompatibleBrands = new[] { "efgh" } };

            PiffWriter.WriteBox(output, box, ctx);

            var bytes = ms.GetBuffer().Take((int) ms.Length).ToArray();
            TestUtil.Compare(ftyp, bytes);
        }


        [TestMethod]
        public void Writer_WriteNullBox()
        {
            var ctx = new PiffWriteContext();
            var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);

            PiffWriter.WriteBox(output, null, ctx);

            Assert.AreEqual(0L, ms.Length);
        }


        [TestMethod]
        public void Writer_WriteBoxNewId()
        {
            var ftyp = new byte[] {
                0, 0, 0, 20, 0x73, 0x6b, 0x69, 0x70, 0x61, 0x62, 0x63, 0x64, 0, 0, 0, 10,
                0x65, 0x66, 0x67, 0x68 };

            var ctx = new PiffWriteContext();
            var ms = new MemoryStream();
            using var output = new BitWriteStream(ms, true);
            var box = new PiffFileTypeBox { MajorBrand = "abcd", MinorVersion = 10, CompatibleBrands = new[] { "efgh" } };
            box.BoxType = "skip";

            PiffWriter.WriteBox(output, box, ctx);

            var bytes = ms.GetBuffer().Take((int) ms.Length).ToArray();
            TestUtil.Compare(ftyp, bytes);
        }


        [TestMethod]
        public void Writer_WriteSuperSpeedwayHeader()
        {
            var ctx = new PiffWriteContext();
            using var ms = new MemoryStream();
            PiffWriter.WriteHeader(ms, SpeedwayManifest, ctx);

            var actual = ms.GetBuffer().Take((int)ms.Length).ToArray();
            var expected = File.ReadAllBytes("Data/SuperSpeedway_720_230.ismv");
            TestUtil.Compare(expected, actual);
        }


        [TestMethod]
        public void Writer_WriteFooter()
        {
            var audio = new List<PiffSampleOffsetDto>
            {
                new PiffSampleOffsetDto{Time = 0, Offset = 10},
                new PiffSampleOffsetDto{Time = 100, Offset = 1000},
            };
            var video = new List<PiffSampleOffsetDto>
            {
                new PiffSampleOffsetDto{Time = 0, Offset = 500},
                new PiffSampleOffsetDto{Time = 120, Offset = 2000},
            };

            var ctx = new PiffWriteContext();
            using var ms = new MemoryStream();
            PiffWriter.WriteFooter(ms, new PiffManifest(), audio, video, ctx);

            Assert.AreEqual(148, ms.Length);

            var actual = ms.GetBuffer().Take((int)ms.Length).ToArray();

            var firstLen = actual.GetInt32(0);
            var lastLen = actual.GetInt32(actual.Length - 4);
            // "mfra" takes the whole footer
            Assert.AreEqual(ms.Length, firstLen);
            // "mfro" keeps the length of "mfra"
            Assert.AreEqual(firstLen, lastLen);
        }


        [TestMethod]
        public void Writer_MinimalFile()
        {
            var ctx = new PiffWriteContext();
            using var output = new MemoryStream();

            PiffWriter.WriteHeader(output, SpeedwayManifest, ctx);

            // Data chunks are MOOF boxes
            var audioChunk = new byte[] { 1, 1, 1, 1 };
            var audioOffset = output.Position;
            output.Write(audioChunk, 0, audioChunk.Length);

            var videoChunk = new byte[] { 2, 2, 2, 2 };
            var videoOffset = output.Position;
            output.Write(videoChunk, 0, videoChunk.Length);

            var audioOffsets = new[] { new PiffSampleOffsetDto { Time = 0, Offset = (ulong) audioOffset } };
            var videoOffsets = new[] { new PiffSampleOffsetDto { Time = 0, Offset = (ulong) videoOffset } };
            PiffWriter.WriteFooter(output, SpeedwayManifest, audioOffsets, videoOffsets, ctx);

            Assert.AreEqual(2492, output.Length);
        }
    }
}