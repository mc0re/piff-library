using System;

namespace PiffLibrary
{
    [BoxName("mdhd")]
    internal class PiffMediaHeader : PiffBoxBase
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
        /// ISO-639-2/T language code stored as 3 5-bit values (ASC - 0x60);
        /// the left-most bit must be 0.
        /// </summary>
        /// <remarks>0x55C4 = "und"</remarks>
        public short Language { get; } = 0x55C4;


        public short Reserved1 { get; } = 0;

        #endregion


        #region Init and clean-up

        public PiffMediaHeader(DateTime created, long duration, int timeScale)
        {
            CreationTime = PiffWriter.GetSecondsFromEpoch(created);
            ModificationTime = CreationTime;
            TimeScale = timeScale;
            Duration = duration;
        }

        #endregion
    }
}