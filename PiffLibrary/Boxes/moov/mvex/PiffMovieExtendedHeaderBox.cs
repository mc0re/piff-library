namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Duration of the movie, if known.
    /// </summary>
    [BoxName("mehd")]
    public sealed class PiffMovieExtendedHeaderBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Length of the whole movie including fragments
        /// in <see cref="PiffMovieHeaderBox.TimeScale"/> units.
        /// This is the duration of the longest track.
        /// </summary>
        [PiffDataFormat(nameof(GetDurationFormat))]
        public ulong Duration { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieExtendedHeaderBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieExtendedHeaderBox(ulong duration)
        {
            Version = 1;
            Duration = duration;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetDurationFormat()
        {
            return Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;
        }

        #endregion
    }
}