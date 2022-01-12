using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("trep")]
    [ChildType(typeof(PiffAlternativeStartupSequenceBox))]
    [ChildType(typeof(PiffCompositionToDecodeBox))]
    internal class PiffTrackExtensionProperties : PiffFullBoxBase
    {
        /// <summary>
        /// Track for which the extension properties are provided (as child boxes).
        /// </summary>
        public uint TrackId { get; set; }
    }
}