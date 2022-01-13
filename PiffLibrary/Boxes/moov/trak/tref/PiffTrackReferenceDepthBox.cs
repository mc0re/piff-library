namespace PiffLibrary.Boxes
{
    /// <summary>
    /// This track contains depth information for the referenced video track.
    /// </summary>
    [BoxName("vdep")]
    public sealed class PiffTrackReferenceDepthBox : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
