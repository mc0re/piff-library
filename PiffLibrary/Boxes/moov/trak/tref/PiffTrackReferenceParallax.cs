namespace PiffLibrary.Boxes
{
    /// <summary>
    /// This track contains parallax information for the referenced video track.
    /// </summary>
    [BoxName("vplx")]
    public sealed class PiffTrackReferenceParallax : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
