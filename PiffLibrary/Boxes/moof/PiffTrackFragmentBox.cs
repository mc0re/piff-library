namespace PiffLibrary.Boxes
{
    [BoxName("traf")]
    [ChildType(typeof(PiffSubSampleInformationBox))]
    [ChildType(typeof(PiffSampleAuxiliaryInformationBox))]
    [ChildType(typeof(PiffSampleAuxiliaryOffsetBox))]
    [ChildType(typeof(PiffTrackFragmentHeaderBox))]
    [ChildType(typeof(PiffTrackFragmentRunBox))]
    [ChildType(typeof(PiffTrackFragmentDecodeTimeBox))]
    [ChildType(typeof(PiffSampleToGroupBox))]
    [ChildType(typeof(PiffSampleGroupDescriptionBox))]
    public sealed class PiffTrackFragmentBox : PiffBoxBase
    {
    }
}