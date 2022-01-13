namespace PiffLibrary.Boxes
{
    [BoxName("msrc")]
    public sealed class PiffTrackGroupMultiSourceBox : PiffFullBoxBase
    {
        public uint TrackGroupId { get; set; }
    }
}