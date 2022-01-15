namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Maps presentation timeline to media timeline.
    /// </summary>
    [BoxName("edts")]
    [ChildType(typeof(PiffEditListBox))]
    public sealed class PiffEditBox : PiffBoxBase
    {
    }
}
