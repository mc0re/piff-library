using System;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// How to decrypt the data.
    /// </summary>
    [BoxName("sinf")]
    [ChildType(typeof(PiffOriginalFormatBox))]
    [ChildType(typeof(PiffSchemeTypeBox))]
    [ChildType(typeof(PiffSchemeInformationBox))]
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
            PiffOriginalFormatBox format,
            PiffSchemeTypeBox schema,
            Guid keyId)
        {
            Children = new PiffBoxBase[] { format, schema, new PiffSchemeInformationBox(keyId) };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemeInformationBox CreateAudio(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformationBox(PiffOriginalFormatBox.CreateAudio(codecId),
                                                       PiffSchemeTypeBox.CreateAudio(),
                                                       keyId);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemeInformationBox CreateVideo(string codecId, Guid keyId)
        {
            return new PiffProtectionSchemeInformationBox(PiffOriginalFormatBox.CreateVideo(codecId),
                                                       PiffSchemeTypeBox.CreateVideo(),
                                                       keyId);
        }

        #endregion
    }
}