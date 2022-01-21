namespace PiffLibrary.Boxes
{
    public sealed class PiffChannelLayoutChannelItem
    {
        #region Constants

        private const int ExplicitPosition = 126;

        #endregion


        #region Properties

        /// <summary>
        /// OutputChannelPosition from ISO/IEC 23001‐8
        /// </summary>
        public byte SpeakerPosition { get; set; }


        /// <summary>
        /// Degrees.
        /// </summary>
        [PiffDataFormat(nameof(UseAzimuth))]
        public short Azimuth { get; set; }


        /// <summary>
        /// Degrees.
        /// </summary>
        [PiffDataFormat(nameof(UseElevation))]
        public sbyte Elevation { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats UseAzimuth() =>
            SpeakerPosition == ExplicitPosition ? PiffDataFormats.Int16 : PiffDataFormats.Skip;


        private PiffDataFormats UseElevation() =>
            SpeakerPosition == ExplicitPosition ? PiffDataFormats.Int8 : PiffDataFormats.Skip;

        #endregion
    }
}