namespace PiffLibrary
{
    [BoxName("mfhd")]
    internal class PiffMovieFragmentHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Fragment sequence number. Starts with 1 and increases throughout the file.
        /// </summary>
        public uint Sequence { get; set; }

        #endregion
    }
}