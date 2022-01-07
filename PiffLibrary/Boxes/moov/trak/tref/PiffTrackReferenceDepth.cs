namespace PiffLibrary
{
    /// <summary>
    /// This track contains depth information for the referenced video track.
    /// </summary>
    [BoxName("vdep")]
    internal class PiffTrackReferenceDepth
    {
        public uint[] TrackIds { get; set; }
    }
}
