namespace PiffLibrary.Boxes
{
    [BoxName("trep")]
    [ChildType(typeof(PiffAlternativeStartupSequenceBox))]
    [ChildType(typeof(PiffCompositionToDecodeBox))]
    public sealed class PiffTrackExtensionPropertiesBox : PiffFullBoxBase
    {
        /// <summary>
        /// Track for which the extension properties are provided (as child boxes).
        /// </summary>
        public uint TrackId { get; set; }
    }
}