namespace PiffLibrary.Boxes
{
    [BoxName("strk")]
    [ChildType(typeof(PiffSubTrackInformationBox))]
    [ChildType(typeof(PiffSubTrackDefinitionBox))]
    public sealed class PiffSubTrackBox : PiffBoxBase
    {
    }
}
