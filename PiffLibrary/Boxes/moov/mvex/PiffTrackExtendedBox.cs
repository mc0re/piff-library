namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Defaults for fragments <see cref="PiffMovieFragmentBox"/> / <see cref="PiffTrackFragmentBox"/>.
    /// </summary>
    [BoxName("trex")]
    public sealed class PiffTrackExtendedBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Matches track ID in <see cref="PiffMovieBox"/>.
        /// </summary>
        public uint TrackId { get; set; }


        public uint DefaultDescriptionIndex { get; set; }


        public uint DefaultSampleDuration { get; set; }


        public uint DefaultSampleSize { get; set; }


        /// <summary>
        /// Default sample flags for the whole track.
        /// 
        /// 4 bits - reserved (0)
        /// 8 bits - sample flags as in <see cref="PiffSampleDependencyBox.Dependencies"/>
        /// 3 bits - padding as <see cref="PiffPaddingBitsBox.Padding"/>
        /// 1 bit - samples are non-sync, see <see cref="PiffSyncSampleBox"/>
        /// </summary>
        public ushort DefaultSampleFlags { get; set; }

        /// <summary>
        /// Default for <see cref="PiffDegradationPriorityBox"/>.
        /// </summary>
        public ushort DefaultDegradationPriority { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffTrackExtendedBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffTrackExtendedBox(uint trackId)
        {
            DefaultDescriptionIndex = 1;
            TrackId = trackId;
        }

        #endregion
    }
}