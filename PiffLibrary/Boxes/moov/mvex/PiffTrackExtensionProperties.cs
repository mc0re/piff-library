namespace PiffLibrary
{
    [BoxName("trep")]
    [ChildType(typeof(PiffAlternativeStartupSequence))]
    [ChildType(typeof(PiffCompositionToDecode))]
    internal class PiffTrackExtensionProperties : PiffFullBoxBase
    {
        /// <summary>
        /// Track for which the extension properties are provided (as child boxes).
        /// </summary>
        public uint TrackId { get; set; }
    }
}