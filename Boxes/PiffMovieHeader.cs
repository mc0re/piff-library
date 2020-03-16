using System;

namespace PiffLibrary
{
    [BoxName("mvhd")]
    internal class PiffMovieHeader
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit time and duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; } = 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;

        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        public long CreationTime { get; }

        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        public long ModificationTime { get; }

        /// <summary>
        /// How many units are in 1 second.
        /// </summary>
        public int TimeScale { get; }

        /// <summary>
        /// Longest track duration in <see cref="TimeScale"/>.
        /// </summary>
        public long Duration { get; }

        /// <summary>
        /// 16.16 fixed point.
        /// </summary>
        public int Rate { get; } = 0x00010000;

        /// <summary>
        /// 8.8 fixed point.
        /// </summary>
        public short Volume { get; } = 0x0100;

        public short Reserved1 { get; } = 0;

        public int[] Reserved2 { get; } = { 0, 0 };

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

        public int[] Predefined3 { get; } = { 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Tracks are numbered from 1.
        /// Assuming there are two tracks.
        /// </summary>
        public int NextTrackId { get; } = 3;

        #endregion


        #region Init and clean-up

        public PiffMovieHeader(DateTime created, TimeSpan duration, int timeScale)
        {
            CreationTime = PiffWriter.GetSecondsFromEpoch(created);
            ModificationTime = CreationTime;
            TimeScale = timeScale;
            Duration = PiffWriter.GetTicks(duration, timeScale);
        }

        #endregion
    }
}