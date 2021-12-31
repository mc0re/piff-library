using System;

namespace PiffLibrary
{
    [BoxName("sinf")]
    internal class PiffProtectionSchemeInformation : PiffBoxBase
    {
        #region Properties

        public PiffProtectionOriginalFormat Format { get; set; }

        public PiffProtectionSchemaType Schema { get; set; }

        public PiffProtectionSchemeInfo Info { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionSchemeInformation()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffProtectionSchemeInformation(
            PiffProtectionOriginalFormat format,
            PiffProtectionSchemaType schema,
            Guid keyId)
        {
            Format = format;
            Schema = schema;
            Info = new PiffProtectionSchemeInfo(keyId);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemeInformation CreateAudio(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformation(PiffProtectionOriginalFormat.CreateAudio(codecId),
                                                       PiffProtectionSchemaType.CreateAudio(),
                                                       keyId);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemeInformation CreateVideo(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformation(PiffProtectionOriginalFormat.CreateVideo(codecId),
                                                       PiffProtectionSchemaType.CreateVideo(),
                                                       keyId);
        }

        #endregion
    }
}