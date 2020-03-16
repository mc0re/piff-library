using System;

namespace PiffLibrary
{
    [BoxName("sinf")]
    internal class PiffProtectionSchemeInformation
    {
        #region Properties

        public PiffProtectionOriginalFormat Format { get; }

        public PiffProtectionSchemaType Schema { get; }

        public PiffProtectionSchemeInfo Info { get; }

        #endregion


        #region Init and clean-up

        private PiffProtectionSchemeInformation(
            PiffProtectionOriginalFormat format,
            PiffProtectionSchemaType schema,
            Guid keyId)
        {
            Format = format;
            Schema = schema;
            Info = new PiffProtectionSchemeInfo(keyId);
        }


        public static PiffProtectionSchemeInformation CreateAudio(Guid keyId)
        {
            return new PiffProtectionSchemeInformation(PiffProtectionOriginalFormat.CreateAudio(),
                                                       PiffProtectionSchemaType.CreateAudio(),
                                                       keyId);
        }


        public static PiffProtectionSchemeInformation CreateVideo(Guid keyId)
        {
            return new PiffProtectionSchemeInformation(PiffProtectionOriginalFormat.CreateVideo(),
                                                       PiffProtectionSchemaType.CreateVideo(),
                                                       keyId);
        }

        #endregion
    }
}