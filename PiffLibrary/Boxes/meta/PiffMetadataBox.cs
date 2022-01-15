namespace PiffLibrary.Boxes
{
    [BoxName("meta")]
    [ChildType(typeof(PiffHandlerTypeBox))]
    [ChildType(typeof(PiffDataInformationBox))]
    [ChildType(typeof(PiffXmlBox))]
    [ChildType(typeof(PiffBinaryXmlBox))]
    [ChildType(typeof(PiffItemLocationBox))]
    public sealed class PiffMetadataBox : PiffFullBoxBase
    {
    }
    public sealed class PiffPrimaryItemBox{}
    public sealed class PiffItemProtectionBox{}
    public sealed class PiffItemInfoBox{}
    public sealed class PiffIPMPControlBox{}
    public sealed class PiffItemReferenceBox{}
    public sealed class PiffItemDataBox{}
}
