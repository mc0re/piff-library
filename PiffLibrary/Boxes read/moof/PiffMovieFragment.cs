namespace PiffLibrary
{
    [BoxName("moof")]
    internal class PiffMovieFragment : PiffBoxBase
    {
        public PiffMovieFragmentHeader Header { get; set; }

        public PiffTrackFragment Track { get; set; }
    }
}
