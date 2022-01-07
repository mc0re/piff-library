namespace PiffLibrary
{
    /// <summary>
    /// The opposite of <see cref="PiffTrackReferenceHint"/>.
    /// </summary>
    [BoxName("hind")]
    internal class PiffTrackReferenceHintDependency
    {
        public uint[] TrackIds { get; set; }
    }
}
