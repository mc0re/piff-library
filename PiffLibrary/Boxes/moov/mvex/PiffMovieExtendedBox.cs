using System.Linq;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// There might be <see cref="PiffMovieFragmentBox"/> boxes in this file.
    /// </summary>
    [BoxName("mvex")]
    [ChildType(typeof(PiffMovieExtendedHeaderBox))]
    [ChildType(typeof(PiffTrackExtendedBox))]
    [ChildType(typeof(PiffLevelAssignmentBox))]
    [ChildType(typeof(PiffTrackExtensionPropertiesBox))]
    public sealed class PiffMovieExtendedBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieExtendedBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieExtendedBox(ulong duration, uint nofTracks)
        {
            Childen =
                new PiffBoxBase[] { new PiffMovieExtendedHeaderBox(duration) }
                .Concat(
                Enumerable.Range(1, (int) nofTracks).Select(t => new PiffTrackExtendedBox((uint) t)))
                .ToArray();
        }

        #endregion
    }
}