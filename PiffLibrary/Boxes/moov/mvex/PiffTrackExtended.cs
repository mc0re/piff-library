using PiffLibrary.Boxes;

namespace PiffLibrary
{
    /// <summary>
    /// Defaults for fragments <see cref="PiffMovieFragment"/> / <see cref="PiffTrackFragment"/>.
    /// </summary>
    [BoxName("trex")]
    internal class PiffTrackExtended : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Matches track ID in <see cref="PiffMovieMetadata"/>.
        /// </summary>
        public uint TrackId { get; set; }


        public uint DefaultDescriptionIndex { get; set; }


        public uint DefaultDuration { get; set; }


        public uint DefaultSize { get; set; }


        /// <summary>
        /// Default sample flags for the whole track.
        /// 
        /// 4 bits - reserved (0)
        /// 8 bits - sample flags as in <see cref="PiffSampleDependency.Dependencies"/>
        /// 3 bits - padding as <see cref="PiffPaddingBits.Padding"/>
        /// 1 bit - samples are non-sync, see <see cref="PiffSyncSample"/>
        /// </summary>
        public ushort DefaultFlags { get; set; }

        /// <summary>
        /// Default for <see cref="PiffDegradationPriorityBox"/>.
        /// </summary>
        public ushort DefaultDegradationPriority { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffTrackExtended()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffTrackExtended(uint trackId)
        {
            DefaultDescriptionIndex = 1;
            TrackId = trackId;
        }

        #endregion
    }
}