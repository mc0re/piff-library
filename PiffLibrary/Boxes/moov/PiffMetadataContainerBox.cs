namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Additional metadata container.
    /// </summary>
    [BoxName("meco")]
    [ChildType(typeof(PiffMetadataBox))]
    public sealed class PiffMetadataContainerBox : PiffBoxBase
    {
    }
}
