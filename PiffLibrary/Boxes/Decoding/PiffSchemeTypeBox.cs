namespace PiffLibrary.Boxes
{
    [BoxName("schm")]
    public sealed class PiffSchemeTypeBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Code defining the protection or restriction scheme.
        /// </summary>
        [PiffStringLength(4)]
        public string SchemeType { get; set; }


        public uint SchemeVersion { get; set; }


        /// <summary>
        /// Web page for installing the <see cref="SchemeType"/> handler.
        /// </summary>
        [PiffDataFormat(nameof(GetUrlFormat))]
        public string SchemeUrl { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffSchemeTypeBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffSchemeTypeBox(uint version)
        {
            SchemeType = "piff";
            SchemeVersion = version;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSchemeTypeBox CreateAudio()
        {
            return new PiffSchemeTypeBox(0x10000);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSchemeTypeBox CreateVideo()
        {
            return new PiffSchemeTypeBox(0x10001);
        }

        #endregion


        #region Format API

        private PiffDataFormats GetUrlFormat()
        {
            return (Flags & 1) == 1 ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;
        }

        #endregion
    }
}