namespace PiffLibrary.Boxes
{
    /// <summary>
    /// A set of tracks with a particular feature in common.
    /// </summary>
    [BoxName("trgr")]
    [ChildType(typeof(PiffTrackGroupMultiSourceBox))]
    public sealed class PiffTrackGroupBox : PiffBoxBase
    {
    }
}