namespace PiffLibrary
{
    [BoxName("traf")]
    [ChildType(typeof(PiffSubSampleInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryOffset))]
    [ChildType(typeof(PiffTrackFragmentHeader))]
    [ChildType(typeof(PiffTrackFragmentRun))]
    internal class PiffTrackFragment : PiffBoxBase
    {
    }
}