namespace PiffLibrary.Boxes
{
    /// <summary>
    /// This track contains subtitle or other overlay for the referenced track
    /// or any track in the alternate group.
    /// </summary>
    [BoxName("subt")]
    public sealed class PiffTrackReferenceOverlayBox : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
