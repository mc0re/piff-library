namespace PiffLibrary
{
    [BoxName("mfro")]
    internal class PiffMovieFragmentRandomAccessOffset : PiffFullBoxBase
    {
        #region Properties

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