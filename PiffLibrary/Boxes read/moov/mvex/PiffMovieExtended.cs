using System.Linq;

namespace PiffLibrary
{
    [BoxName("mvex")]
    internal class PiffMovieExtended : PiffBoxBase
    {
        #region Properties

        public PiffMovieExtendedHeader Header { get; set; }

        public PiffTrackExtended[] Track { get; set; }

        #endregion


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
        public PiffMovieExtended(long duration, int nofTracks)
        {
            Header = new PiffMovieExtendedHeader(duration);
            Track = Enumerable.Range(1, nofTracks).Select(t => new PiffTrackExtended(t)).ToArray();
        }

        #endregion
    }
}