using PiffLibrary.Boxes;
using System;

namespace PiffLibrary
{
    [BoxName("mvhd")]
    internal class PiffMovieHeader : PiffFullBoxBase
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
        /// How many units are in 1 second.
        /// </summary>
        public uint TimeScale { get; set; }


        /// <summary>
        /// Longest track duration in <see cref="TimeScale"/>.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public ulong Duration { get; set; }


        /// <summary>
        /// 16.16 fixed point.
        /// </summary>
        public int Rate { get; set; } = 0x00010000;


        /// <summary>
        /// 8.8 fixed point.
        /// </summary>
        public short Volume { get; set; } = 0x0100;


        [PiffArraySize(2)]
        public byte[] Reserved1 { get; } = { 0, 0 };


        [PiffArraySize(2)]
        public int[] Reserved2 { get; } = { 0, 0 };


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


        [PiffArraySize(6)]
        public int[] Predefined3 { get; } = { 0, 0, 0, 0, 0, 0 };


        /// <summary>
        /// Tracks are numbered from 1.
        /// Assuming there are two tracks.
        /// </summary>
        public uint NextTrackId { get; set; } = 3;

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieHeader()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieHeader(DateTime created, ulong duration, uint timeScale)
        {
            Version = 1;
            CreationTime = PiffWriter.GetSecondsFromEpoch(created);
            ModificationTime = CreationTime;
            TimeScale = timeScale;
            Duration = duration;
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