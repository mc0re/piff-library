namespace PiffLibrary.Boxes
{
    /// <summary>
    /// User information about the containing box.
    /// </summary>
    [BoxName("udta")]
    [ChildType(typeof(PiffCopyrightBox))]
    [ChildType(typeof(PiffTrackSelectionBox))]
    [ChildType(typeof(PiffTrackKindBox))]
    [ChildType(typeof(PiffSubTrackBox))]
    [ChildType(typeof(PiffRtpMovieHintInformationBox))]
    [ChildType(typeof(PiffRtpTrackSdpHintInformationBox))]
    [ChildType(typeof(PiffHintStatisticsBox))]
    [ChildType(typeof(PiffLoudnessBox))]
    public sealed class PiffUserDataBox : PiffBoxBase
    {
    }
}
