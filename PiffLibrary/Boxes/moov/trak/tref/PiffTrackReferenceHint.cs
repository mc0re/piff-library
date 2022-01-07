namespace PiffLibrary
{
    /// <summary>
    /// Referenced tracks contain the original media for this track.
    /// </summary>
    [BoxName("hint")]
    internal class PiffTrackReferenceHint
    {
        public uint[] TrackIds { get; set; }
    }
}
