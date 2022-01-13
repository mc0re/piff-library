namespace PiffLibrary.Boxes
{
    [BoxName("schm")]
    public sealed class PiffProtectionSchemaTypeBox : PiffFullBoxBase
    {
        #region Properties

        [PiffStringLength(4)]
        public string SchemaType { get; set; } = "piff";


        public int SchemaVersion { get; set; }


        [PiffDataFormat(nameof(GetUrlFormat))]
        public string SchemaUrl { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionSchemaTypeBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffProtectionSchemaTypeBox(int version)
        {
            SchemaVersion = version;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemaTypeBox CreateAudio()
        {
            return new PiffProtectionSchemaTypeBox(0x10000);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemaTypeBox CreateVideo()
        {
            return new PiffProtectionSchemaTypeBox(0x10001);
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