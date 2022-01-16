namespace PiffLibrary.Boxes
{
    [BoxName("meta")]
    [ChildType(typeof(PiffHandlerTypeBox))]
    [ChildType(typeof(PiffDataInformationBox))]
    [ChildType(typeof(PiffXmlBox))]
    [ChildType(typeof(PiffBinaryXmlBox))]
    [ChildType(typeof(PiffItemLocationBox))]
    [ChildType(typeof(PiffPrimaryItemBox))]
    [ChildType(typeof(PiffItemProtectionBox))]
    [ChildType(typeof(PiffItemInfoBox))]
    [ChildType(typeof(PiffItemDataBox))]
    [ChildType(typeof(PiffItemReferenceBox))]
    [ChildType(typeof(PiffFdItemInformationBox))]
    public sealed class PiffMetadataBox : PiffFullBoxBase
    {
    }
}
