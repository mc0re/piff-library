using System;

namespace PiffLibrary
{
    [BoxName("tkhd")]
    internal class PiffTrackHeader
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit time and duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; } = 1;

        /// <summary>
        /// Flags: 0=enabled, 1=in_movie, 2=in_preview, 3=size_is_aspect_ratio.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 7;

        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        public long CreationTime { get; }

        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        public long ModificationTime { get; }

        public int TrackId { get; }

        public int Reserved1 { get; } = 0;

        /// <summary>
        /// Track duration in <see cref="PiffMovieHeader.TimeScale"/>.
        /// </summary>
        public long Duration { get; }

        public int[] Reserved2 { get; } = { 0, 0 };

        /// <summary>
        /// Z-ordering. 0 is normal, -1 is closer to the viewer than 0, etc.
        /// </summary>
        public short Layer { get; } = 0;

        /// <summary>
        /// Group ID joining multiple tracks with similar meaning.
        /// </summary>
        public short AlternateGroup { get; } = 0;

        /// <summary>
        /// 8.8 fixed point.
        /// </summary>
        public short Volume { get; }

        public short Reserved3 { get; } = 0;

        /// <summary>
        /// Order: {a, b, u, c, d, v, x, y, w}.
        /// a-d and x-y stored as 16.16 fixed point.
        /// u, v, w stored as 2.30 fixed point.
        /// </summary>
        public int[] TransformationMatrix { get; } = {
            0x10000, 0, 0,
            0, 0x10000, 0,
            0, 0, 0x40000000
        };

        /// <summary>
        /// 16.16 fixed point.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 16.16 fixed point.
        /// </summary>
        public int Height { get; }

        #endregion


        #region Init and clean-up

        private PiffTrackHeader(
            DateTime created, long duration,
            int trackId, PiffTrackTypes trackType, short width, short height)
        {
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
                Width = width << 16;
                Height = height << 16;
            }
        }


        public static PiffTrackHeader CreateAudio(byte trackId, DateTime created, long duration)
        {
            return new PiffTrackHeader(created, duration, trackId, PiffTrackTypes.Audio, 0, 0);
        }


        public static PiffTrackHeader CreateVideo(byte trackId, DateTime created, PiffVideoManifest video)
        {
            return new PiffTrackHeader(created, video.Duration, trackId, PiffTrackTypes.Video,
                                       video.Width, video.Height);
        }

        #endregion
    }
}