namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Mark the track role or kind.
    /// </summary>
    [BoxName("kind")]
    public sealed class PiffTrackKindBox : PiffFullBoxBase
    {
        /// <summary>
        /// If <see cref="Value"/> is present this is the naming scheme for <see cref="Value"/>.
        /// If not, identifier of the track kind.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string Uri { get; set; }


        /// <summary>
        /// Name from the declared by <see cref="Uri"/> scheme.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string Value { get; set; }
    }
}
