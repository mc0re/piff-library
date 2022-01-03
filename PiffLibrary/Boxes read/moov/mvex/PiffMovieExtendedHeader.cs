namespace PiffLibrary
{
    [BoxName("mehd")]
    internal class PiffMovieExtendedHeader : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; set; } = 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        /// <summary>
        /// Length of the whole movie including fragments.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public long Duration { get; set; }

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
        public PiffMovieExtendedHeader(long duration)
        {
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