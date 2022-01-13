namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Referenced tracks contain the fonts for this track.
    /// </summary>
    [BoxName("font")]
    public sealed class PiffTrackReferenceFontBox : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
