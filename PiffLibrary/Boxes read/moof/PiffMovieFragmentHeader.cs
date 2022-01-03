namespace PiffLibrary
{
    [BoxName("mfhd")]
    internal class PiffMovieFragmentHeader : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        /// <summary>
        /// Fragment sequence number.
        /// </summary>
        public int Sequence { get; set; }

        #endregion
    }
}