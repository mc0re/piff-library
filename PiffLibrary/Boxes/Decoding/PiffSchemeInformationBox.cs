using System;


namespace PiffLibrary.Boxes
{
    [BoxName("schi")]
    [ChildType(typeof(PiffExtensionBox))] // Expects PiffProtectionTrackEncryption
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
            Childen = new[] { PiffExtensionBox.ProtectionTrackEncryption(keyId) };
        }

        #endregion
    }
}