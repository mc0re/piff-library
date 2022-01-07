namespace PiffLibrary
{
    [BoxName("mfhd")]
    internal class PiffMovieFragmentHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Fragment sequence number.
        /// </summary>
        public int Sequence { get; set; }

        #endregion
    }
}