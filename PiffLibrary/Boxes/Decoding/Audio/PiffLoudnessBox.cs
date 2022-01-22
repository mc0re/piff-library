namespace PiffLibrary.Boxes
{
    [BoxName("ludt")]
    [ChildType(typeof(PiffTrackLoudnessInfoBox))]
    [ChildType(typeof(PiffAlbumLoudnessInfoBox))]
    public sealed class PiffLoudnessBox : PiffBoxBase
    {
    }
}
