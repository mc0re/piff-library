using PiffLibrary.Boxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PiffLibrary
{
    /// <summary>
    /// Reads the edit lists ("moov/trak/edts/elst") of an MP4 file.
    /// </summary>
    public static class PiffEditListAnalyzer
    {
        #region Constants

        /// <summary>
        /// Number of parser messages quoted in an exception message.
        /// </summary>
        private const int MaxQuotedMessages = 10;

        #endregion


        #region Fields

        /// <summary>
        /// Box types an MP4 file starts with; a file starting otherwise is not parsed.
        /// </summary>
        private static readonly HashSet<string> mFirstBoxTypes = new HashSet<string>
        {
            "ftyp", "styp", "moov", "mdat", "free", "skip", "wide", "pdin", "sidx", "moof", "pnot", "uuid", "junk", "prfl"
        };

        #endregion


        #region API

        /// <summary>
        /// Edit list of every track in the file's "moov" box, in file order.
        /// </summary>
        /// <remarks>
        /// The media data is skipped, not read. A file whose first box is not an MP4 box gives a report without tracks.
        /// </remarks>
        /// <param name="tolerance">Media time below which an edit list is trivial, per track type.</param>
        /// <exception cref="InvalidDataException">The file starts like an MP4, but has no single readable "moov" box.</exception>
        public static PiffEditListReport Read(string path, PiffEditListTolerance tolerance)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (tolerance == null) throw new ArgumentNullException(nameof(tolerance));

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Read(stream, path, tolerance);
            }
        }


        /// <summary>
        /// Edit list of every track of the MP4 file in <paramref name="stream"/>, which must be seekable and at its start.
        /// </summary>
        /// <param name="name">File name for the exception message.</param>
        /// <inheritdoc cref="Read(string, PiffEditListTolerance)" path="/param[@name='tolerance']"/>
        /// <inheritdoc cref="Read(string, PiffEditListTolerance)" path="/remarks"/>
        /// <inheritdoc cref="Read(string, PiffEditListTolerance)" path="/exception"/>
        public static PiffEditListReport Read(Stream stream, string name, PiffEditListTolerance tolerance)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (tolerance == null) throw new ArgumentNullException(nameof(tolerance));

            var header = new byte[8];
            var read = ReadAtLeast(stream, header);
            stream.Position = 0;
            var headerText = ToPrintable(header, read);
            var isMp4 = read == header.Length && mFirstBoxTypes.Contains(Encoding.ASCII.GetString(header, 4, 4));

            if (!isMp4) return new PiffEditListReport(headerText, null, Array.Empty<string>(), tolerance);

            var ctx = new PiffReadContext { SkipMediaData = true };
            PiffMovieBox[] moovs;

            try
            {
                moovs = PiffFile.Parse(stream, ctx).Boxes.OfType<PiffMovieBox>().ToArray();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(CannotReadMessage(name, ex.Message, ctx), ex);
            }

            if (moovs.Length != 1)
            {
                var reason = moovs.Length == 0 ? "no 'moov' box" : $"{moovs.Length} 'moov' boxes";

                throw new InvalidDataException(CannotReadMessage(name, reason, ctx));
            }

            var tracks =
                (moovs[0].ChildrenOfType<PiffTrackBox>() ?? Array.Empty<PiffTrackBox>())
                .Select((trak, index) => ReadTrack(trak, index + 1))
                .ToList();

            return new PiffEditListReport(headerText, tracks, ctx.Messages.ToList(), tolerance);
        }

        #endregion


        #region Utility

        /// <summary>
        /// Fill <paramref name="buffer"/> from the stream; fewer bytes only at the end of the stream.
        /// </summary>
        /// <returns>The number of bytes read.</returns>
        private static int ReadAtLeast(Stream stream, byte[] buffer)
        {
            var total = 0;

            while (total < buffer.Length)
            {
                var read = stream.Read(buffer, total, buffer.Length - total);
                if (read == 0) break;

                total += read;
            }

            return total;
        }


        /// <summary>
        /// Bytes as ASCII text, non-printable characters replaced by dots.
        /// </summary>
        private static string ToPrintable(byte[] data, int count) =>
            new string(data.Take(count).Select(b => b >= 0x20 && b < 0x7F ? (char) b : '.').ToArray());


        /// <summary>
        /// Message for a file whose MP4 structure cannot be read, quoting the first parser messages.
        /// </summary>
        private static string CannotReadMessage(string name, string reason, PiffReadContext ctx)
        {
            var message = $"Cannot read the MP4 structure of '{name}': {reason}.";

            if (ctx.Messages.Count > 0)
            {
                message += " Parser messages: " + string.Join("; ", ctx.Messages.Take(MaxQuotedMessages));
            }

            return message;
        }


        /// <summary>
        /// Track ID, handler type, media timescale and edit list of one "trak" box.
        /// </summary>
        private static PiffEditListInfo ReadTrack(PiffTrackBox trak, int position)
        {
            var trackId = trak.FirstOfType<PiffTrackHeaderBox>()?.TrackId ?? (uint) position;
            var timeScale = trak.FirstOfType<PIffTrackMediaInfoBox, PiffMediaHeaderBox>()?.TimeScale ?? 0;
            var handlerType = trak.FirstOfType<PIffTrackMediaInfoBox, PiffHandlerTypeBox>()?.HandlerType;
            var elst = trak.FirstOfType<PiffEditBox, PiffEditListBox>();
            var first = elst?.Entries?.FirstOrDefault();

            return new PiffEditListInfo(
                trackId, elst?.EntryCount ?? 0, first?.MediaTime ?? 0, first?.MediaRate ?? PiffEditListInfo.NormalRate, timeScale,
                handlerType);
        }

        #endregion
    }
}
