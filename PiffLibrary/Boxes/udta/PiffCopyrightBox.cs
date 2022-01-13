namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Copyright declaration for the entire presentation or a track.
    /// </summary>
    [BoxName("cprt")]
    public sealed class PiffCopyrightBox : PiffFullBoxBase
    {
        /// <summary>
        /// Same as <see cref="PiffMediaHeaderBox.Language"/>.
        /// </summary>
        public short Language { get; set; }


        [PiffDataFormat(PiffDataFormats.Utf8Or16Zero)]
        public string Notice { get; set; }
    }
}
