namespace PiffLibrary
{
    [BoxName("trex")]
    internal class PiffTrackExtended
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        public int TrackId { get; }


        public int DefaultDescriptionIndex { get; } = 1;


        public int DefaultDuration { get; } = 0;


        public int DefaultSize { get; } = 0;


        public int DefaultFlags { get; } = 0;

        #endregion


        #region Init and clean-up

        public PiffTrackExtended(int trackId)
        {
            TrackId = trackId;
        }

        #endregion
    }
}