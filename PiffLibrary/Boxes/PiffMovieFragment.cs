namespace PiffLibrary
{
    [BoxName("moof")]
    internal class PiffMovieFragment : PiffBoxBase
    {
        public PiffMovieFragmentHeader Header { get; internal set; }

        public PiffTrackFragment Track { get; internal set; }
    }
}
