namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Referenced tracks contain the original media for this track.
    /// </summary>
    [BoxName("hint")]
    public sealed class PiffTrackReferenceHintBox : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
