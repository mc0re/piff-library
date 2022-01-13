namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Usually is put in the end of <see cref="PiffMovieFragmentRandomAccessBox"/>
    /// for length verificarion and for simplifying seeking (look from the end of the file).
    /// </summary>
    [BoxName("mfro")]
    public sealed class PiffMovieFragmentRandomAccessOffsetBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Size of the enclosing <see cref="PiffMovieFragmentRandomAccessBox"/> box.
        /// </summary>
        public uint MfraSize { get; set; }

        #endregion
    }
}