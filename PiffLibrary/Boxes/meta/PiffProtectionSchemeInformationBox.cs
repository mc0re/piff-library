using System;


namespace PiffLibrary.Boxes
{
    [BoxName("sinf")]
    [ChildType(typeof(PiffProtectionOriginalFormatBox))]
    [ChildType(typeof(PiffProtectionSchemaTypeBox))]
    [ChildType(typeof(PiffProtectionSchemeInfoBox))]
    public sealed class PiffProtectionSchemeInformationBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionSchemeInformationBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffProtectionSchemeInformationBox(
            PiffProtectionOriginalFormatBox format,
            PiffProtectionSchemaTypeBox schema,
            Guid keyId)
        {
            Childen = new PiffBoxBase[] { format, schema, new PiffProtectionSchemeInfoBox(keyId) };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemeInformationBox CreateAudio(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformationBox(PiffProtectionOriginalFormatBox.CreateAudio(codecId),
                                                       PiffProtectionSchemaTypeBox.CreateAudio(),
                                                       keyId);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemeInformationBox CreateVideo(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformationBox(PiffProtectionOriginalFormatBox.CreateVideo(codecId),
                                                       PiffProtectionSchemaTypeBox.CreateVideo(),
                                                       keyId);
        }

        #endregion
    }
}