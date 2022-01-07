namespace PiffLibrary
{
    [BoxName("trex")]
    internal class PiffTrackExtended : PiffFullBoxBase
    {
        #region Properties

        public int TrackId { get; set; }


        public int DefaultDescriptionIndex { get; set; } = 1;


        public int DefaultDuration { get; set; }


        public int DefaultSize { get; set; }


        public int DefaultFlags { get; set; }

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
        public PiffTrackExtended(int trackId)
        {
            TrackId = trackId;
        }

        #endregion
    }
}