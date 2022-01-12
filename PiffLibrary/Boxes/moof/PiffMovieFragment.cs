using PiffLibrary.Boxes;

namespace PiffLibrary
{
    /// <summary>
    /// Additional meta data for the actual streams, placed in <see cref="PiffMediaData"/> boxes.
    /// </summary>
    [BoxName("moof")]
    [ChildType(typeof(PiffMovieFragmentHeader))]
    [ChildType(typeof(PiffTrackFragment))]
    internal class PiffMovieFragment : PiffBoxBase
    {
    }
}
