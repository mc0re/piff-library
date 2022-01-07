using System.Linq;

namespace PiffLibrary
{
    [BoxName("mvex")]
    [ChildType(typeof(PiffMovieExtendedHeader))]
    [ChildType(typeof(PiffTrackExtended))]
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
        public PiffMovieExtended(ulong duration, int nofTracks)
        {
            Childen =
                new PiffBoxBase[] { new PiffMovieExtendedHeader(duration) }
                .Concat(
                Enumerable.Range(1, nofTracks).Select(t => new PiffTrackExtended(t)))
                .ToArray();
        }

        #endregion
    }
}