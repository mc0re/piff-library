using System;

namespace PiffLibrary
{
    [BoxName("sinf")]
    internal class PiffProtectionSchemeInformation : PiffBoxBase
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


        public static PiffProtectionSchemeInformation CreateAudio(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformation(PiffProtectionOriginalFormat.CreateAudio(codecId),
                                                       PiffProtectionSchemaType.CreateAudio(),
                                                       keyId);
        }


        public static PiffProtectionSchemeInformation CreateVideo(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformation(PiffProtectionOriginalFormat.CreateVideo(codecId),
                                                       PiffProtectionSchemaType.CreateVideo(),
                                                       keyId);
        }

        #endregion
    }
}