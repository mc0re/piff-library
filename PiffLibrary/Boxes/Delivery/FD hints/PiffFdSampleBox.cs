namespace PiffLibrary.Boxes
{
    [BoxName("fdsa")]
    [ChildType(typeof(PiffFdPacketBox))]
    [ChildType(typeof(PiffExtraDataBox))]
    public sealed class PiffFdSampleBox : PiffBoxBase
    {
    }
}
