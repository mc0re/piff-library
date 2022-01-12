using PiffLibrary.Boxes;

namespace PiffLibrary
{
    /// <summary>
    /// Duration of the movie, if known.
    /// </summary>
    [BoxName("mehd")]
    internal class PiffMovieExtendedHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Length of the whole movie including fragments
        /// in <see cref="PiffMovieHeader.TimeScale"/> units.
        /// This is the duration of the longest track.
        /// </summary>
        [PiffDataFormat(nameof(GetDurationFormat))]
        public ulong Duration { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieExtendedHeader()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieExtendedHeader(ulong duration)
        {
            Version = 1;
            Duration = duration;
        }

        #endregion
        
        
        #region Format API

        public PiffDataFormats GetDurationFormat()
        {
            return Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;
        }

        #endregion
    }
}