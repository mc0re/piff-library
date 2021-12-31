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
        public byte Version { get; set; } = 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public long CreationTime { get; set; }


        /// <summary>
        /// Seconds since 01.01.1904 00:00 UTC.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public long ModificationTime { get; set; }


        /// <summary>
        /// How many units are in 1 second.
        /// </summary>
        public int TimeScale { get; set; }


        /// <summary>
        /// Longest track duration in <see cref="TimeScale"/>.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public long Duration { get; set; }


        /// <summary>
        /// ISO-639-2/T language code stored as 3 non-zero 5-bit values (ASC - 0x60);
        /// the left-most bit must be 0.
        /// </summary>
        /// <remarks>
        /// 0x15C7 = "eng"
        /// 0x55C4 = "und"
        /// "neu" is aldo "und"
        /// 0x7FFF = "und"
        /// </remarks>
        public short Language { get; set; } = 0x55C4;


        /// <summary>
        /// Not read in Bento4...
        /// </summary>
        public short Reserved1 { get; } = 0;

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMediaHeader()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMediaHeader(DateTime created, long duration, int timeScale)
        {
            CreationTime = PiffWriter.GetSecondsFromEpoch(created);
            ModificationTime = CreationTime;
            TimeScale = timeScale;
            Duration = duration;
        }

        #endregion


        #region Format API

        public PiffDataFormats GetDateFormat()
        {
            return Version == 0 ? PiffDataFormats.Int32 : PiffDataFormats.Int64;
        }

        #endregion
    }
}