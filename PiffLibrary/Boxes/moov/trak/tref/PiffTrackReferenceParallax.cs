namespace PiffLibrary
{
    /// <summary>
    /// This track contains parallax information for the referenced video track.
    /// </summary>
    [BoxName("vplx")]
    internal class PiffTrackReferenceParallax
    {
        public uint[] TrackIds { get; set; }
    }
}
