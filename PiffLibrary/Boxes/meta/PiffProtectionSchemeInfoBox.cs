using System;


namespace PiffLibrary.Boxes
{
    [BoxName("schi")]
    [ChildType(typeof(PiffExtensionBox))] // Expects PiffProtectionTrackEncryption
    public sealed class PiffProtectionSchemeInfoBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionSchemeInfoBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffProtectionSchemeInfoBox(Guid keyId)
        {
            Childen = new[] { PiffExtensionBox.ProtectionTrackEncryption(keyId) };
        }

        #endregion
    }
}