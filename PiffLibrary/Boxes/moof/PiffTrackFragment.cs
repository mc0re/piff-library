namespace PiffLibrary
{
    [BoxName("traf")]
    [ChildType(typeof(PiffSubSampleInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryOffset))]
    internal class PiffTrackFragment : PiffBoxBase
    {
        public PiffTrackFragmentHeader Header { get; set; }
        
        public PiffTrackFragmentRun Run { get; set; }
    }
}