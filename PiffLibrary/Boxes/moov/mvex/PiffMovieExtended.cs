using PiffLibrary.Boxes;
using System.Linq;

namespace PiffLibrary
{
    /// <summary>
    /// There might be <see cref="PiffMovieFragment"/> boxes in this file.
    /// </summary>
    [BoxName("mvex")]
    [ChildType(typeof(PiffMovieExtendedHeader))]
    [ChildType(typeof(PiffTrackExtended))]
    [ChildType(typeof(PiffLevelAssignmentBox))]
    [ChildType(typeof(PiffTrackExtensionProperties))]
    internal class PiffMovieExtended : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieExtended()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieExtended(ulong duration, uint nofTracks)
        {
            Childen =
                new PiffBoxBase[] { new PiffMovieExtendedHeader(duration) }
                .Concat(
                Enumerable.Range(1, (int)nofTracks).Select(t => new PiffTrackExtended((uint)t)))
                .ToArray();
        }

        #endregion
    }
}