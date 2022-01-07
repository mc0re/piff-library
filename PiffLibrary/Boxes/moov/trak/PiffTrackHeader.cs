using System;

namespace PiffLibrary
{
    /// <summary>
    /// Flags:
    /// - 1 = enabled
    /// - 2 = in_movie
    /// - 4 = in_preview
    /// - 8 = size_is_aspect_ratio.
    /// </summary>
    [BoxName("tkhd")]
    internal class PiffTrackHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public ulong CreationTime { get; set; }


        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public ulong ModificationTime { get; set; }


        /// <summary>
        /// 1-based.
        /// </summary>
        public uint TrackId { get; set; }


        public int Reserved1 { get; }


        /// <summary>
        /// Track duration in <see cref="PiffMovieHeader.TimeScale"/>.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public ulong Duration { get; set; }


        [PiffArraySize(2)]
        public int[] Reserved2 { get; } = { 0, 0 };


        /// <summary>
        /// Z-ordering. 0 is normal, -1 is closer to the viewer than 0, etc.
        /// </summary>
        public short Layer { get; set; }


        /// <summary>
        /// Group ID joining multiple tracks with similar meaning (like audio language),
        /// only one track from a group should be played at any one time.
        /// Zero means "no group information".
        /// </summary>
        public short AlternateGroup { get; set; }


        /// <summary>
        /// 8.8 fixed point.
        /// </summary>
        public short Volume { get; set; }


        public short Reserved3 { get; }


        /// <summary>
        /// Order: {a, b, u, c, d, v, x, y, w}.
        /// a-d and x-y stored as 16.16 fixed point.
        /// u, v, w stored as 2.30 fixed point.
        /// </summary>
        [PiffArraySize(9)]
        public int[] TransformationMatrix { get; set; } = {
            0x10000, 0, 0,
            0, 0x10000, 0,
            0, 0, 0x40000000
        };


        /// <summary>
        /// 16.16 fixed point.
        /// </summary>
        public uint Width { get; set; }


        /// <summary>
        /// 16.16 fixed point.
        /// </summary>
        public uint Height { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffTrackHeader()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffTrackHeader(
            DateTime created, ulong duration,
            uint trackId, PiffTrackTypes trackType, ushort width, ushort height)
        {
            Version = 1;
            Flags = 7;
            CreationTime = PiffWriter.GetSecondsFromEpoch(created);
            ModificationTime = CreationTime;
            TrackId = trackId;
            Duration = duration;

            if (trackType == PiffTrackTypes.Audio)
            {
                Volume = 0x100;
                Width = 0;
                Height = 0;
            }
            else
            {
                Volume = 0;
                Width = (uint)width << 16;
                Height = (uint)height << 16;
            }
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrackHeader CreateAudio(uint trackId, DateTime created, ulong duration)
        {
            return new PiffTrackHeader(created, duration, trackId, PiffTrackTypes.Audio, 0, 0);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrackHeader CreateVideo(uint trackId, DateTime created, PiffVideoManifest video)
        {
            return new PiffTrackHeader(created, video.Duration, trackId, PiffTrackTypes.Video,
                                       video.Width, video.Height);
        }

        #endregion


        #region Format API

        public PiffDataFormats GetDateFormat()
        {
            return Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;
        }

        #endregion
    }
}