namespace PiffLibrary
{
    /// <summary>
    /// This track contains subtitle or other overlay for the referenced track
    /// or any track in the alternate group.
    /// </summary>
    [BoxName("subt")]
    internal class PiffTrackReferenceOverlay
    {
        public uint[] TrackIds { get; set; }
    }
}
