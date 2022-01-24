using System;


namespace PiffLibrary.Boxes
{
    [BoxName("schi")]
    [ChildType(typeof(PiffExtensionBox))] // Expects PiffProtectionTrackEncryption
    [ChildType(typeof(PiffStereoVideoBox))]
    [ChildType(typeof(PiffTrackEncryptionBox))]
    public sealed class PiffSchemeInformationBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffSchemeInformationBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffSchemeInformationBox(Guid keyId)
        {
            Children = new[] { PiffExtensionBox.ProtectionTrackEncryption(keyId) };
        }

        #endregion
    }
}