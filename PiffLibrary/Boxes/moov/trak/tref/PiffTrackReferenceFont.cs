namespace PiffLibrary
{
    /// <summary>
    /// Referenced tracks contain the fonts for this track.
    /// </summary>
    [BoxName("font")]
    internal class PiffTrackReferenceFont
    {
        public uint[] TrackIds { get; set; }
    }
}
