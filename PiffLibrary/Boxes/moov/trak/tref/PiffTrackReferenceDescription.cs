namespace PiffLibrary
{
    /// <summary>
    /// This track desribes the referenced tracks.
    /// </summary>
    [BoxName("cdsc")]
    internal class PiffTrackReferenceDescription
    {
        public uint[] TrackIds { get; set; }
    }
}
