using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("traf")]
    [ChildType(typeof(PiffSubSampleInformationBox))]
    [ChildType(typeof(PiffSampleAuxiliaryInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryOffset))]
    [ChildType(typeof(PiffTrackFragmentHeader))]
    [ChildType(typeof(PiffTrackFragmentRunBox))]
    [ChildType(typeof(PiffTrackFragmentDecodeTime))]
    [ChildType(typeof(PiffSampleToGroupBox))]
    [ChildType(typeof(PiffSampleGroupDescriptionBox))]
    internal class PiffTrackFragment : PiffBoxBase
    {
    }
}