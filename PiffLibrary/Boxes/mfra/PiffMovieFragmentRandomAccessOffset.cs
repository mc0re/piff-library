namespace PiffLibrary
{
    /// <summary>
    /// Usually is put in the end of <see cref="PiffMovieFragmentRandomAccess"/>
    /// for length verificarion.
    /// </summary>
    [BoxName("mfro")]
    internal class PiffMovieFragmentRandomAccessOffset : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Size of the enclosing <see cref="PiffMovieFragmentRandomAccess"/> box.
        /// </summary>
        public uint MfraSize { get; set; }

        #endregion
    }
}