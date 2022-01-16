namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Additional metadata container.
    /// </summary>
    [BoxName("meco")]
    [ChildType(typeof(PiffMetadataBox))]
    [ChildType(typeof(PiffMetaboxRelationBox))]
    public sealed class PiffMetadataContainerBox : PiffBoxBase
    {
    }
}
