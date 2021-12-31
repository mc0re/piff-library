namespace PiffLibrary
{
    [BoxName("mfhd")]
    internal class PiffMovieFragmentHeader : PiffBoxBase
    {
        #region Properties

        public byte Version { get; internal set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; internal set; } = 0;


        public int Sequence { get; internal set; }

        #endregion
    }
}