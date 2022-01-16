namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Relative wall-clock times at which movie fragents were produced.
    /// The box is related to the next coming <see cref="PiffMovieFragmentBox"/>.
    /// </summary>
    [BoxName("prft")]
    public sealed class PiffProducerReferenceTimeBox : PiffFullBoxBase
    {
        #region Properties

        public uint ReferenceTrackId { get; set; }


        /// <summary>
        /// UTC time in NTP format.
        /// </summary>
        public ulong NtpTimestamp { get; set; }


        /// <summary>
        /// Same time as <see cref="NtpTimestamp"/>, but in time units
        /// used for the referenced track.
        /// </summary>
        [PiffDataFormat(nameof(GetTimeFormat))]
        public ulong MediaTime { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetTimeFormat() =>
            Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;

        #endregion
    }
}