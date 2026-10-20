using System;
using System.Globalization;

namespace PiffLibrary
{
    /// <summary>
    /// Edit list ("elst") of one track.
    /// </summary>
    public sealed class PiffEditListInfo
    {
        #region Constants

        /// <summary>
        /// Media rate 1.0 in 16.16 fixed point.
        /// </summary>
        public const int NormalRate = 0x10000;


        /// <summary>
        /// Handler type of a video track.
        /// </summary>
        public const string VideoHandlerType = "vide";


        /// <summary>
        /// Handler type of an audio track.
        /// </summary>
        public const string AudioHandlerType = "soun";

        #endregion


        #region Properties

        /// <summary>
        /// Track ID from "tkhd", or the 1-based position of the track when it has none.
        /// </summary>
        public uint TrackId { get; }


        /// <summary>
        /// Number of edit list entries; 0 when the track has no edit list.
        /// </summary>
        public uint EntryCount { get; }


        /// <summary>
        /// Media time of the first entry in the track's timescale; -1 is an empty edit.
        /// </summary>
        public long MediaTime { get; }


        /// <summary>
        /// Media rate of the first entry, 16.16 fixed point; <see cref="NormalRate"/> is 1.0, 0 is a dwell.
        /// </summary>
        public int MediaRate { get; }


        /// <summary>
        /// Media timescale of the track from "mdhd", units per second; 0 when unknown.
        /// </summary>
        public uint TimeScale { get; }


        /// <summary>
        /// Handler type of the track from "mdia/hdlr", e.g. <see cref="VideoHandlerType"/> or <see cref="AudioHandlerType"/>;
        /// <see langword="null"/> when unknown.
        /// </summary>
        public string HandlerType { get; }


        /// <summary>
        /// Whether this is an audio track.
        /// </summary>
        public bool IsAudio => HandlerType == AudioHandlerType;


        /// <summary>
        /// <see cref="MediaTime"/> in real time; <see langword="null"/> for an empty edit or an unknown timescale.
        /// </summary>
        public TimeSpan? MediaTimeSpan
        {
            get
            {
                if (TimeScale == 0 || MediaTime < 0) return null;

                var ticks = MediaTime * (double) TimeSpan.TicksPerSecond / TimeScale;

                return ticks < TimeSpan.MaxValue.Ticks ? TimeSpan.FromTicks((long) ticks) : TimeSpan.MaxValue;
            }
        }


        /// <summary>
        /// Short description, e.g. "track 2, media time 1024 (23.2 ms)".
        /// </summary>
        public string Description =>
            EntryCount > 1
            ? $"track {TrackId}, {EntryCount} entries, {MediaTimeText}"
            : MediaRate != NormalRate
                ? $"track {TrackId}, {MediaTimeText}, media rate {(MediaRate / (double) NormalRate).ToString(CultureInfo.InvariantCulture)}"
                : $"track {TrackId}, {MediaTimeText}";


        /// <summary>
        /// Raw media time, followed by the real time when known.
        /// </summary>
        private string MediaTimeText =>
            MediaTimeSpan.HasValue
            ? $"media time {MediaTime} ({MediaTimeSpan.Value.TotalMilliseconds.ToString("0.0", CultureInfo.InvariantCulture)} ms)"
            : $"media time {MediaTime}";

        #endregion


        #region Init and clean-up

        public PiffEditListInfo(
            uint trackId, uint entryCount, long mediaTime, int mediaRate = NormalRate, uint timeScale = 0, string handlerType = null)
        {
            TrackId = trackId;
            EntryCount = entryCount;
            MediaTime = mediaTime;
            MediaRate = mediaRate;
            TimeScale = timeScale;
            HandlerType = handlerType;
        }

        #endregion


        #region API

        /// <summary>
        /// Whether the edit list shifts, cuts or holds the media: more than one entry, a media rate other than 1.0,
        /// or a media time that is negative, of unknown real time, or not below <paramref name="tolerance"/>.
        /// </summary>
        public bool IsNonTrivial(TimeSpan tolerance) =>
            EntryCount > 1 ||
            (EntryCount == 1 && (MediaRate != NormalRate || (MediaTime != 0 && !(MediaTimeSpan < tolerance))));


        /// <summary>
        /// <see cref="IsNonTrivial(TimeSpan)"/> at the tolerance for this track's <see cref="HandlerType"/>.
        /// </summary>
        public bool IsNonTrivial(PiffEditListTolerance tolerance)
        {
            if (tolerance == null) throw new ArgumentNullException(nameof(tolerance));

            return IsNonTrivial(tolerance.For(HandlerType));
        }


        public override string ToString() => Description;

        #endregion
    }
}
