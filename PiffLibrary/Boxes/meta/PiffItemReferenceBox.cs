namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Links one item to others via typed references.
    /// </summary>
    [BoxName("iref")]
    [ChildType(typeof(PiffItemReferenceItemBox))]
    public sealed class PiffItemReferenceBox : PiffFullBoxBase
    {
    }
}
