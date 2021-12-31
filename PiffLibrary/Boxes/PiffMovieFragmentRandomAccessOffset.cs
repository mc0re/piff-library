namespace PiffLibrary
{
    [BoxName("mfro")]
    internal class PiffMovieFragmentRandomAccessOffset : PiffBoxBase
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        public int MfraSize { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieFragmentRandomAccessOffset(int mfraSize)
        {
            MfraSize = mfraSize;
        }

        #endregion
    }
}