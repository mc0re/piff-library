using PiffLibrary.Boxes;

namespace PiffLibrary
{
    /// <summary>
    /// A set of tracks with a particular feature in common.
    /// </summary>
    [BoxName("trgr")]
    [ChildType(typeof(PiffTrackGroupMultiSource))]
    internal class PiffTrackGroup : PiffBoxBase
    {
    }
}