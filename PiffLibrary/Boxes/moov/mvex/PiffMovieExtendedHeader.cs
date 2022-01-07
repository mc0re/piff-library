namespace PiffLibrary
{
    [BoxName("mehd")]
    internal class PiffMovieExtendedHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Length of the whole movie including fragments.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public ulong Duration { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieExtendedHeader()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieExtendedHeader(ulong duration)
        {
            Version = 1;
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