namespace PiffLibrary
{
    [BoxName("schm")]
    internal class PiffProtectionSchemaType
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        public string SchemaType { get; } = "piff";


        public int SchemaVersion { get; }

        #endregion


        #region Init and clean-up

        private PiffProtectionSchemaType(int version)
        {
            SchemaVersion = version;
        }


        public static PiffProtectionSchemaType CreateAudio()
        {
            return new PiffProtectionSchemaType(0x10000);
        }


        public static PiffProtectionSchemaType CreateVideo()
        {
            return new PiffProtectionSchemaType(0x10001);
        }

        #endregion
    }
}