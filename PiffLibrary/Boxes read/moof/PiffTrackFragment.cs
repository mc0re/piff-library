namespace PiffLibrary
{
    [BoxName("traf")]
    internal class PiffTrackFragment : PiffBoxBase
    {
        public PiffTrackFragmentHeader Header { get; set; }
        
        public PiffTrackFragmentRun Run { get; set; }
    }
}