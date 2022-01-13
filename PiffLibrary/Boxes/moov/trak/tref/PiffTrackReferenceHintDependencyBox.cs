namespace PiffLibrary.Boxes
{
    /// <summary>
    /// The opposite of <see cref="PiffTrackReferenceHintBox"/>.
    /// </summary>
    [BoxName("hind")]
    public sealed class PiffTrackReferenceHintDependencyBox : PiffBoxBase
    {
        public uint[] TrackIds { get; set; }
    }
}
