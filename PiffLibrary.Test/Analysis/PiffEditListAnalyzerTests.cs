using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Text;


namespace PiffLibrary.Test
{
    [TestClass]
    public class PiffEditListAnalyzerTests
    {
        #region Sparse stream

        /// <summary>
        /// Read-only stream of a large file: a head, a gap of zeros and a tail. Counts the bytes read.
        /// </summary>
        private sealed class SparseStream : Stream
        {
            private readonly byte[] mHead;


            private readonly byte[] mTail;


            private readonly long mLength;


            private long mPosition;


            public SparseStream(byte[] head, long gap, byte[] tail)
            {
                mHead = head;
                mTail = tail;
                mLength = head.Length + gap + tail.Length;
            }


            public long BytesRead { get; private set; }


            public override bool CanRead => true;

            public override bool CanSeek => true;

            public override bool CanWrite => false;

            public override long Length => mLength;

            public override long Position
            {
                get => mPosition;
                set => mPosition = value;
            }


            public override int Read(byte[] buffer, int offset, int count)
            {
                var available = (int) Math.Max(0, Math.Min(count, mLength - mPosition));

                for (var i = 0; i < available; i++)
                {
                    buffer[offset + i] = ByteAt(mPosition + i);
                }

                mPosition += available;
                BytesRead += available;

                return available;
            }


            public override long Seek(long offset, SeekOrigin origin) =>
                mPosition = origin switch
                {
                    SeekOrigin.Begin => offset,
                    SeekOrigin.Current => mPosition + offset,
                    _ => mLength + offset,
                };

            public override void Flush() { }

            public override void SetLength(long value) => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();


            private byte ByteAt(long position) =>
                position < mHead.Length
                ? mHead[position]
                : position >= mLength - mTail.Length
                    ? mTail[position - (mLength - mTail.Length)]
                    : (byte) 0;
        }

        #endregion


        #region Constants

        private const int NormalRate = PiffEditListInfo.NormalRate;


        private static readonly PiffEditListTolerance DefaultTolerance =
            new PiffEditListTolerance(TimeSpan.FromMilliseconds(40), TimeSpan.FromMilliseconds(100));

        #endregion


        #region Fields

        private string? mWorkDir;

        #endregion


        #region Init and clean-up

        [TestInitialize]
        public void Setup()
        {
            mWorkDir = Path.Combine(Path.GetTempPath(), "piff-tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(mWorkDir);
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (mWorkDir != null && Directory.Exists(mWorkDir)) Directory.Delete(mWorkDir, true);
        }

        #endregion


        #region Tests

        [TestMethod]
        public void EditListAnalyzer_TwoTracks_NonTrivialOnTrack1()
        {
            var file = Concat(FileType(), Clip());

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "clip.mp4", DefaultTolerance);

            Assert.IsTrue(report.IsMp4);
            Assert.AreEqual(2, report.Tracks.Count);
            AssertTrack(report.Tracks[0], 1, 1, 2000, NormalRate, 30000);
            Assert.IsTrue(report.Tracks[0].IsNonTrivial(report.Tolerance));
            AssertTrack(report.Tracks[1], 2, 1, 0, NormalRate, 22050);
            Assert.IsFalse(report.Tracks[1].IsNonTrivial(report.Tolerance));
            Assert.AreEqual("vide", report.Tracks[0].HandlerType);
            Assert.AreEqual("soun", report.Tracks[1].HandlerType);
            Assert.IsTrue(report.Tracks[1].IsAudio);
            Assert.AreSame(DefaultTolerance, report.Tolerance);
            Assert.AreEqual("track 1, media time 2000 (66.7 ms)", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        [DataRow(100, false, DisplayName = "Audio tolerance 100 ms")]
        [DataRow(40, true, DisplayName = "Audio tolerance 40 ms")]
        public void EditListAnalyzer_AudioPrimingAt22050_AudioTolerance(int audioMs, bool nonTrivial)
        {
            var file = Concat(FileType(), Box("moov", Track(0, 30000, handlerType: "vide"), Track(1024, 22050, handlerType: "soun")));
            var tolerance = new PiffEditListTolerance(TimeSpan.FromMilliseconds(40), TimeSpan.FromMilliseconds(audioMs));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "audio.mp4", tolerance);

            Assert.AreEqual(nonTrivial, report.Tracks[1].IsNonTrivial(report.Tolerance));
            Assert.AreEqual(nonTrivial ? "track 2, media time 1024 (46.4 ms)" : null, report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        [DataRow("vide", DisplayName = "Video")]
        [DataRow("text", DisplayName = "Other handler")]
        [DataRow(null, DisplayName = "No handler")]
        public void EditListAnalyzer_NonAudioTrackAt46Ms_VideoTolerance_NonTrivial(string? handlerType)
        {
            var file = Concat(FileType(), Box("moov", Track(1024, 22050, handlerType: handlerType)));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "video.mp4", DefaultTolerance);

            Assert.AreEqual(handlerType, report.Tracks[0].HandlerType);
            Assert.AreEqual("track 1, media time 1024 (46.4 ms)", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListTolerance_For_Ok()
        {
            var tolerance = new PiffEditListTolerance(TimeSpan.FromMilliseconds(40), TimeSpan.FromMilliseconds(100));

            Assert.AreEqual(TimeSpan.FromMilliseconds(40), tolerance.For("vide"));
            Assert.AreEqual(TimeSpan.FromMilliseconds(100), tolerance.For("soun"));
            Assert.AreEqual(TimeSpan.FromMilliseconds(40), tolerance.For("subt"));
            Assert.AreEqual(TimeSpan.FromMilliseconds(40), tolerance.For(null));
        }


        [TestMethod]
        public void EditListAnalyzer_TrackHeaderIds_Used()
        {
            var file = Concat(FileType(), Box("moov", Track(0, 30000, trackId: 7), Track(1024, 22050, trackId: 9)));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "ids.mp4", DefaultTolerance);

            Assert.AreEqual(7u, report.Tracks[0].TrackId);
            Assert.AreEqual(9u, report.Tracks[1].TrackId);
            Assert.AreEqual("track 9, media time 1024 (46.4 ms)", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListAnalyzer_TextFile_NotMp4()
        {
            var text = Path.Combine(mWorkDir!, "notes.txt");
            File.WriteAllText(text, "Hello world, this is not a video.\n");

            var report = PiffEditListAnalyzer.Read(text, DefaultTolerance);

            Assert.IsFalse(report.IsMp4);
            Assert.IsNull(report.Tracks);
            Assert.IsNull(report.FirstNonTrivial);
            Assert.AreEqual("Hello wo", report.Header);
        }


        [TestMethod]
        public void EditListAnalyzer_RiffFile_NotMp4()
        {
            var riff = Concat(Encoding.ASCII.GetBytes("RIFF"), [0x24, 0, 0, 0], Encoding.ASCII.GetBytes("WAVEfmt "));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(riff), "sound.wav", DefaultTolerance);

            Assert.IsFalse(report.IsMp4);
            Assert.AreEqual("RIFF$...", report.Header);
        }


        [TestMethod]
        public void EditListAnalyzer_ShortFile_NotMp4()
        {
            var report = PiffEditListAnalyzer.Read(new MemoryStream(Encoding.ASCII.GetBytes("ftyp")), "short.mp4", DefaultTolerance);

            Assert.IsFalse(report.IsMp4);
            Assert.AreEqual("ftyp", report.Header);
        }


        [TestMethod]
        public void EditListAnalyzer_PnotBeforeFtyp_Parsed()
        {
            var file = Concat(Box("pnot", new byte[12]), FileType(), Clip());

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "pnot.mov", DefaultTolerance);

            Assert.IsTrue(report.IsMp4);
            Assert.AreEqual(2, report.Tracks.Count);
            Assert.AreEqual("track 1, media time 2000 (66.7 ms)", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListAnalyzer_EmptyEditBoxOnTrack1_Track2NonTrivial()
        {
            var moov = Box("moov", Box("trak", Box("edts")), Box("trak", Box("edts", EditList(2000, NormalRate))));
            var file = Concat(FileType(), moov);

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "synthetic.mp4", DefaultTolerance);

            Assert.AreEqual(2, report.Tracks.Count);
            AssertTrack(report.Tracks[0], 1, 0, 0, NormalRate, 0);
            AssertTrack(report.Tracks[1], 2, 1, 2000, NormalRate, 0);
            Assert.AreEqual("track 2, media time 2000", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListAnalyzer_Dwell_NonTrivial()
        {
            var moov = Box("moov", Box("trak", Box("edts", EditList(0, 0))));
            var file = Concat(FileType(), moov);

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "dwell.mp4", DefaultTolerance);

            AssertTrack(report.Tracks[0], 1, 1, 0, 0, 0);
            Assert.IsTrue(report.Tracks[0].IsNonTrivial(report.Tolerance));
            Assert.AreEqual("track 1, media time 0, media rate 0", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        [DataRow(1919, 48000, 40, false, DisplayName = "39.98 ms at 48000")]
        [DataRow(1920, 48000, 40, true, DisplayName = "40 ms at 48000")]
        [DataRow(1199, 30000, 40, false, DisplayName = "39.97 ms at 30000")]
        [DataRow(1201, 30000, 40, true, DisplayName = "40.03 ms at 30000")]
        [DataRow(2000, 30000, 40, true, DisplayName = "15 fps frame at 40 ms")]
        [DataRow(2000, 30000, 100, false, DisplayName = "15 fps frame at 100 ms")]
        [DataRow(1024, 44100, 40, false, DisplayName = "AAC priming at 44100")]
        [DataRow(1024, 22050, 40, true, DisplayName = "AAC priming at 22050")]
        public void EditListAnalyzer_MediaTimeAgainstTolerance_Ok(int mediaTime, int timeScale, int toleranceMs, bool nonTrivial)
        {
            var file = Concat(FileType(), Box("moov", Track(mediaTime, (uint) timeScale)));

            var tolerance = new PiffEditListTolerance(TimeSpan.FromMilliseconds(toleranceMs));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "tolerance.mp4", tolerance);

            AssertTrack(report.Tracks[0], 1, 1, mediaTime, NormalRate, (uint) timeScale);
            Assert.AreEqual(nonTrivial, report.Tracks[0].IsNonTrivial(report.Tolerance));
            Assert.AreEqual(nonTrivial, report.FirstNonTrivial != null);
        }


        [TestMethod]
        public void EditListAnalyzer_AacPriming_DescriptionInMilliseconds()
        {
            var file = Concat(FileType(), Box("moov", Track(0, 30000), Track(1024, 44100)));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "priming.mp4", new PiffEditListTolerance(TimeSpan.FromMilliseconds(10)));

            Assert.AreEqual(TimeSpan.FromTicks(232199), report.Tracks[1].MediaTimeSpan);
            Assert.AreEqual("track 2, media time 1024 (23.2 ms)", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        [DataRow(-1, 48000, NormalRate, "track 1, media time -1", DisplayName = "Empty edit")]
        [DataRow(0, 48000, 0, "track 1, media time 0 (0.0 ms), media rate 0", DisplayName = "Dwell")]
        [DataRow(1, 0, NormalRate, "track 1, media time 1", DisplayName = "Zero timescale")]
        public void EditListAnalyzer_LargeTolerance_StillNonTrivial(int mediaTime, int timeScale, int mediaRate, string description)
        {
            var file = Concat(FileType(), Box("moov", Track(mediaTime, (uint) timeScale, mediaRate)));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "odd.mp4", new PiffEditListTolerance(TimeSpan.FromHours(1)));

            Assert.AreEqual(description, report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListAnalyzer_NoMediaHeader_RawRule()
        {
            var file = Concat(FileType(), Box("moov", Box("trak", Box("edts", EditList(1, NormalRate)))));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "no-mdhd.mp4", new PiffEditListTolerance(TimeSpan.FromHours(1)));

            Assert.IsNull(report.Tracks[0].MediaTimeSpan);
            Assert.AreEqual("track 1, media time 1", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListAnalyzer_TwoEntries_NonTrivial()
        {
            var file = Concat(FileType(), Box("moov", Box("trak", Box("edts", EditList([0, 0], NormalRate)), Box("mdia", MediaHeader(48000)))));

            var report = PiffEditListAnalyzer.Read(new MemoryStream(file), "two.mp4", new PiffEditListTolerance(TimeSpan.FromHours(1)));

            Assert.AreEqual("track 1, 2 entries, media time 0 (0.0 ms)", report.FirstNonTrivial?.Description);
        }


        [TestMethod]
        public void EditListAnalyzer_NoMovieBox_Throws()
        {
            var file = Concat(FileType(), Box("free", new byte[4]));

            var ex = Assert.ThrowsException<InvalidDataException>(
                () => PiffEditListAnalyzer.Read(new MemoryStream(file), "empty.mp4", DefaultTolerance));

            Assert.AreEqual("Cannot read the MP4 structure of 'empty.mp4': no 'moov' box.", ex.Message);
        }


        [TestMethod]
        public void EditListAnalyzer_TwoMovieBoxes_Throws()
        {
            var file = Concat(FileType(), Clip(), Clip());

            var ex = Assert.ThrowsException<InvalidDataException>(
                () => PiffEditListAnalyzer.Read(new MemoryStream(file), "twice.mp4", DefaultTolerance));

            StringAssert.StartsWith(ex.Message, "Cannot read the MP4 structure of 'twice.mp4': 2 'moov' boxes.");
        }


        [TestMethod]
        public void EditListAnalyzer_MediaDataBeyondEnd_Throws()
        {
            var mdat = new byte[8];
            BinaryPrimitives.WriteUInt32BigEndian(mdat, 0x7FFFFFFF);
            Encoding.ASCII.GetBytes("mdat").CopyTo(mdat, 4);
            var file = Concat(FileType(), mdat, Clip());

            var ex = Assert.ThrowsException<InvalidDataException>(
                () => PiffEditListAnalyzer.Read(new MemoryStream(file), "truncated.mp4", DefaultTolerance));

            StringAssert.StartsWith(ex.Message, "Cannot read the MP4 structure of 'truncated.mp4': no 'moov' box.");
        }


        [TestMethod]
        public void EditListAnalyzer_LargeMediaData_NotRead()
        {
            const long payloadSize = 3L * 1024 * 1024 * 1024;
            var moov = Clip();
            var mdat = new byte[16];
            BinaryPrimitives.WriteUInt32BigEndian(mdat, 1);
            Encoding.ASCII.GetBytes("mdat").CopyTo(mdat, 4);
            BinaryPrimitives.WriteUInt64BigEndian(mdat.AsSpan(8), (ulong) (mdat.Length + payloadSize));
            var head = Concat(FileType(), mdat);
            var stream = new SparseStream(head, payloadSize, moov);

            var report = PiffEditListAnalyzer.Read(stream, "large.mp4", DefaultTolerance);

            Assert.AreEqual("track 1, media time 2000 (66.7 ms)", report.FirstNonTrivial?.Description);
            Assert.IsTrue(stream.BytesRead < head.Length + moov.Length + 4096, $"Read {stream.BytesRead} bytes.");
        }

        #endregion


        #region Utility

        private static void AssertTrack(PiffEditListInfo track, uint trackId, uint entryCount, long mediaTime, int mediaRate, uint timeScale)
        {
            Assert.AreEqual(
                (trackId, entryCount, mediaTime, mediaRate, timeScale),
                (track.TrackId, track.EntryCount, track.MediaTime, track.MediaRate, track.TimeScale));
        }


        /// <summary>
        /// A box of the given type around the given content.
        /// </summary>
        private static byte[] Box(string type, params byte[][] content)
        {
            var box = Concat([new byte[4], Encoding.ASCII.GetBytes(type), .. content]);
            BinaryPrimitives.WriteUInt32BigEndian(box, (uint) box.Length);

            return box;
        }


        /// <summary>
        /// A minimal "ftyp" box.
        /// </summary>
        private static byte[] FileType() => Box("ftyp", Encoding.ASCII.GetBytes("isom"), new byte[4], Encoding.ASCII.GetBytes("isom"));


        /// <summary>
        /// A "moov" box like the one of a 15 fps clip: video track 1 starting at 2000 / 30000 s, audio track 2 at 0.
        /// </summary>
        private static byte[] Clip() =>
            Box("moov", Track(2000, 30000, trackId: 1, handlerType: "vide"), Track(0, 22050, trackId: 2, handlerType: "soun"));


        /// <summary>
        /// A version 0 "hdlr" box with the given handler type.
        /// </summary>
        private static byte[] Handler(string handlerType) =>
            Box("hdlr", new byte[8], Encoding.ASCII.GetBytes(handlerType), new byte[12], Encoding.ASCII.GetBytes("Track\0"));


        /// <summary>
        /// A version 0 "elst" box with one entry.
        /// </summary>
        private static byte[] EditList(int mediaTime, int mediaRate) => EditList([mediaTime], mediaRate);


        /// <summary>
        /// A version 0 "elst" box with one entry per media time, all at the same media rate.
        /// </summary>
        private static byte[] EditList(int[] mediaTimes, int mediaRate)
        {
            var body = new byte[8 + 12 * mediaTimes.Length];
            BinaryPrimitives.WriteUInt32BigEndian(body.AsSpan(4), (uint) mediaTimes.Length);

            for (var i = 0; i < mediaTimes.Length; i++)
            {
                BinaryPrimitives.WriteUInt32BigEndian(body.AsSpan(8 + 12 * i), 1000);
                BinaryPrimitives.WriteInt32BigEndian(body.AsSpan(12 + 12 * i), mediaTimes[i]);
                BinaryPrimitives.WriteInt32BigEndian(body.AsSpan(16 + 12 * i), mediaRate);
            }

            return Box("elst", body);
        }


        /// <summary>
        /// A version 0 "mdhd" box with the given media timescale.
        /// </summary>
        private static byte[] MediaHeader(uint timeScale)
        {
            var body = new byte[24];
            BinaryPrimitives.WriteUInt32BigEndian(body.AsSpan(12), timeScale);
            BinaryPrimitives.WriteUInt32BigEndian(body.AsSpan(16), timeScale);
            BinaryPrimitives.WriteUInt16BigEndian(body.AsSpan(20), 0x55C4);

            return Box("mdhd", body);
        }


        /// <summary>
        /// A version 0 "tkhd" box with the given track ID.
        /// </summary>
        private static byte[] TrackHeader(uint trackId)
        {
            var body = new byte[84];
            BinaryPrimitives.WriteUInt32BigEndian(body.AsSpan(12), trackId);

            return Box("tkhd", body);
        }


        /// <summary>
        /// A "trak" box with a one-entry edit list and a "mdia/mdhd" carrying the media timescale;
        /// with a "tkhd" when <paramref name="trackId"/> is given and a "mdia/hdlr" when <paramref name="handlerType"/> is.
        /// </summary>
        private static byte[] Track(
            int mediaTime, uint timeScale, int mediaRate = NormalRate, uint? trackId = null, string? handlerType = null)
        {
            var edts = Box("edts", EditList(mediaTime, mediaRate));
            var mdia = handlerType == null
                ? Box("mdia", MediaHeader(timeScale))
                : Box("mdia", MediaHeader(timeScale), Handler(handlerType));

            return trackId.HasValue
                ? Box("trak", TrackHeader(trackId.Value), edts, mdia)
                : Box("trak", edts, mdia);
        }


        /// <summary>
        /// The parts one after the other.
        /// </summary>
        private static byte[] Concat(params byte[][] parts) => parts.SelectMany(p => p).ToArray();

        #endregion
    }
}
