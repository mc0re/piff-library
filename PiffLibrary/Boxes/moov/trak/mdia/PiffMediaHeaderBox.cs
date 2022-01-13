using System;


namespace PiffLibrary.Boxes
{
    [BoxName("mdhd")]
    public sealed class PiffMediaHeaderBox : PiffFullBoxBase
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
        public short Reserved { get; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMediaHeaderBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMediaHeaderBox(DateTime created, ulong duration, uint timeScale)
        {
            Version = 1;
            CreationTime = PiffWriter.GetSecondsFromEpoch(created);
            ModificationTime = CreationTime;
            TimeScale = timeScale;
            Duration = duration;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetDateFormat()
        {
            return Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;
        }

        #endregion
    }
}