namespace PiffLibrary
{
    [BoxName("trex")]
    internal class PiffTrackExtended : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        public int TrackId { get; set; }


        public int DefaultDescriptionIndex { get; set; } = 1;


        public int DefaultDuration { get; set; } = 0;


        public int DefaultSize { get; set; } = 0;


        public int DefaultFlags { get; set; } = 0;

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