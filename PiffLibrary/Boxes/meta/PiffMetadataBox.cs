namespace PiffLibrary.Boxes
{
    [BoxName("meta")]
    [ChildType(typeof(PiffHandlerTypeBox))]
    [ChildType(typeof(PiffDataInformationBox))]
    public sealed class PiffMetadataBox : PiffBoxBase
    {
    }
}
