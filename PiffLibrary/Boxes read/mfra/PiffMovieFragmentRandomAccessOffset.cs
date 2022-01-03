namespace PiffLibrary
{
    [BoxName("mfro")]
    internal class PiffMovieFragmentRandomAccessOffset : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        public int MfraSize { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieFragmentRandomAccessOffset()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieFragmentRandomAccessOffset(int mfraSize)
        {
            MfraSize = mfraSize;
        }

        #endregion
    }
}