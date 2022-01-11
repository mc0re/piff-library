namespace PiffLibrary
{
    [BoxName("traf")]
    [ChildType(typeof(PiffSubSampleInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryOffset))]
    [ChildType(typeof(PiffTrackFragmentHeader))]
    [ChildType(typeof(PiffTrackFragmentRun))]
    [ChildType(typeof(PiffTrackFragmentDecodeTime))]
    [ChildType(typeof(PiffSampleToGroup))]
    [ChildType(typeof(PiffSampleGroupDescription))]
    internal class PiffTrackFragment : PiffBoxBase
    {
    }
}