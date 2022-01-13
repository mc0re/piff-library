namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Additional meta data for the actual streams, placed in <see cref="PiffMediaDataBox"/> boxes.
    /// </summary>
    [BoxName("moof")]
    [ChildType(typeof(PiffMovieFragmentHeaderBox))]
    [ChildType(typeof(PiffTrackFragmentBox))]
    public sealed class PiffMovieFragmentBox : PiffBoxBase
    {
    }
}
