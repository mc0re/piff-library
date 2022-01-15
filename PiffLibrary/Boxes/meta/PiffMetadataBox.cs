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
    public sealed class PiffMetadataBox : PiffFullBoxBase
    {
    }
    public sealed class PiffIPMPControlBox{}
    public sealed class PiffItemReferenceBox{}
    public sealed class PiffItemDataBox{}
}
