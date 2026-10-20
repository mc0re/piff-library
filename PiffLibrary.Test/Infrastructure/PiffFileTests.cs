using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiffLibrary.Boxes;
using System;
using System.IO;
using System.Linq;
using System.Text;


namespace PiffLibrary.Test
{
    [TestClass]
    public class PiffFileTests
    {
        #region Stream counting the bytes read

        private sealed class CountingStream : Stream
        {
            private readonly Stream mUnderlying;


            private readonly bool mCanSeek;


            public long BytesRead { get; private set; }


            public CountingStream(Stream underlying, bool canSeek = true)
            {
                mUnderlying = underlying;
                mCanSeek = canSeek;
            }


            public override bool CanRead => true;

            public override bool CanSeek => mCanSeek;

            public override bool CanWrite => false;

            public override long Length => mUnderlying.Length;

            public override long Position
            {
                get => mUnderlying.Position;
                set => mUnderlying.Position = value;
            }


            public override int Read(byte[] buffer, int offset, int count)
            {
                var read = mUnderlying.Read(buffer, offset, count);
                BytesRead += read;

                return read;
            }


            public override long Seek(long offset, SeekOrigin origin) =>
                mCanSeek ? mUnderlying.Seek(offset, origin) : throw new NotSupportedException();

            public override void Flush() { }

            public override void SetLength(long value) => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }

        #endregion


        #region Constants

        private const int MediaDataSize = 1_000_000;

        #endregion


        #region Tests

        [TestMethod]
        public void File_SkipMediaData_PayloadNotRead()
        {
            using var input = new CountingStream(new MemoryStream(CreateFile(MediaDataSize), false));
            var ctx = new PiffReadContext { SkipMediaData = true };

            var piff = PiffFile.Parse(input, ctx);

            Assert.IsFalse(ctx.IsError, string.Join("; ", ctx.Messages));
            Assert.IsTrue(input.BytesRead < 1000, $"Read {input.BytesRead} bytes.");
            Assert.IsNull(piff.GetSingleBox<PiffMediaDataBox>().RawData);

            var track = piff.GetSingleBox<PiffMovieBox>().ChildrenOfType<PiffTrackBox>().Single();
            Assert.AreEqual(7u, track.FirstOfType<PiffTrackHeaderBox>().TrackId);
            Assert.AreEqual(2000L, track.FirstOfType<PiffEditBox, PiffEditListBox>().Entries[0].MediaTime);
        }


        [TestMethod]
        public void File_NoSkipMediaData_PayloadRead()
        {
            using var input = new CountingStream(new MemoryStream(CreateFile(MediaDataSize), false));
            var ctx = new PiffReadContext();

            var piff = PiffFile.Parse(input, ctx);

            Assert.IsTrue(input.BytesRead >= MediaDataSize, $"Read {input.BytesRead} bytes.");
            Assert.AreEqual(1, piff.Boxes.OfType<PiffMovieBox>().Count());
        }


        [TestMethod]
        public void File_FreeBox_Read()
        {
            using var input = new MemoryStream(CreateFile(16), false);

            var piff = PiffFile.Parse(input, new PiffReadContext());

            Assert.AreEqual("free", piff.GetSingleBox<PiffSkipBox>().BoxType);
            Assert.AreEqual(4, piff.Boxes.Count);
        }


        [TestMethod]
        public void File_SkipMediaData_NonSeekable_Ok()
        {
            using var input = new CountingStream(new MemoryStream(CreateFile(MediaDataSize), false), false);
            var ctx = new PiffReadContext { SkipMediaData = true };

            var piff = PiffFile.Parse(input, ctx);

            Assert.IsFalse(ctx.IsError, string.Join("; ", ctx.Messages));
            Assert.IsNull(piff.GetSingleBox<PiffMediaDataBox>().RawData);

            var track = piff.GetSingleBox<PiffMovieBox>().ChildrenOfType<PiffTrackBox>().Single();
            Assert.AreEqual(7u, track.FirstOfType<PiffTrackHeaderBox>().TrackId);
        }


        [TestMethod]
        public void File_FreeBox_WriteRoundTrip_Ok()
        {
            var original =
                Box("ftyp", Encoding.ASCII.GetBytes("isom\0\0\0\0isom"))
                .Concat(Box("free", new byte[] { 1, 2, 3, 4 }))
                .ToArray();
            var piff = PiffFile.Parse(new MemoryStream(original, false), new PiffReadContext());
            var ctx = new PiffWriteContext();
            using var output = new MemoryStream();

            foreach (var box in piff.Boxes)
            {
                PiffWriter.WriteBox(output, box, ctx);
            }

            var reread = PiffFile.Parse(new MemoryStream(output.ToArray(), false), new PiffReadContext());

            Assert.AreEqual(0, ctx.Errors.Count, string.Join("; ", ctx.Errors));
            CollectionAssert.AreEqual(original, output.ToArray());
            Assert.AreEqual("free", reread.GetSingleBox<PiffSkipBox>().BoxType);
        }


        [TestMethod]
        public void File_EmptyEditBox_FirstOfType_Null()
        {
            using var input = new MemoryStream(CreateFile(16, false), false);

            var piff = PiffFile.Parse(input, new PiffReadContext());

            var movie = piff.GetSingleBox<PiffMovieBox>();
            var track = movie.ChildrenOfType<PiffTrackBox>().Single();
            Assert.IsNull(track.FirstOfType<PiffEditBox>().Children);
            Assert.IsNull(track.FirstOfType<PiffEditBox, PiffEditListBox>());
            Assert.IsNull(movie.FirstOfType<PiffTrackBox, PiffEditBox, PiffEditListBox>());
        }


        [TestMethod]
        public void File_SkipMediaData_TextFile_NoMovie()
        {
            var text = Encoding.ASCII.GetBytes(string.Concat(Enumerable.Repeat("Hello, this is not an MP4 file.\n", 100)));
            using var input = new MemoryStream(text, false);

            var piff = PiffFile.Parse(input, new PiffReadContext { SkipMediaData = true });

            Assert.AreEqual(0, piff.Boxes.OfType<PiffMovieBox>().Count());
        }

        #endregion


        #region Utility

        /// <summary>
        /// A file with "ftyp", "free", "mdat" of the given size and "moov" with one track (ID 7) and an "edts",
        /// holding an edit list unless <paramref name="withEditList"/> is cleared.
        /// </summary>
        private static byte[] CreateFile(int mediaDataSize, bool withEditList = true)
        {
            var tkhd = new byte[84];
            tkhd[15] = 7;

            var elst = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0x0E, 0x10, 0, 0, 0x07, 0xD0, 0, 1, 0, 0 };

            var ftyp = Box("ftyp", Encoding.ASCII.GetBytes("isom\0\0\0\0isom"));
            var mdat = Box("mdat", new byte[mediaDataSize]);
            var edts = withEditList ? Box("edts", Box("elst", elst)) : Box("edts");
            var moov = Box("moov", Box("trak", Box("tkhd", tkhd), edts));

            return ftyp.Concat(Box("free")).Concat(mdat).Concat(moov).ToArray();
        }


        private static byte[] Box(string type, params byte[][] payload)
        {
            var body = payload.SelectMany(p => p).ToArray();
            var size = body.Length + 8;
            var header = new byte[] { (byte) (size >> 24), (byte) (size >> 16), (byte) (size >> 8), (byte) size };

            return header.Concat(Encoding.ASCII.GetBytes(type)).Concat(body).ToArray();
        }

        #endregion
    }
}
