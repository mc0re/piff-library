namespace PiffLibrary.Boxes
{
    /// <summary>
    /// This track desribes the referenced tracks.
    /// </summary>
    [BoxName("cdsc")]
    public sealed class PiffTrackReferenceDescriptionBox : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
