namespace PiffLibrary
{
    [BoxName("traf")]
    [ChildType(typeof(PiffSubSampleInformation))]
    internal class PiffTrackFragment : PiffBoxBase
    {
        public PiffTrackFragmentHeader Header { get; set; }
        
        public PiffTrackFragmentRun Run { get; set; }
    }
}