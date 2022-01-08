namespace PiffLibrary
{
    [BoxName("mfro")]
    internal class PiffMovieFragmentRandomAccessOffset : PiffFullBoxBase
    {
        #region Properties

        public uint MfraSize { get; set; }

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
        public PiffMovieFragmentRandomAccessOffset(uint mfraSize)
        {
            MfraSize = (uint)mfraSize;
        }

        #endregion
    }
}