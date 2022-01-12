using PiffLibrary.Boxes;
using System;

namespace PiffLibrary
{
    [BoxName("schi")]
    [ChildType(typeof(PiffExtensionBox))] // Expects PiffProtectionTrackEncryption
    internal class PiffProtectionSchemeInfo : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionSchemeInfo()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffProtectionSchemeInfo(Guid keyId)
        {
            Childen = new[] { PiffExtensionBox.ProtectionTrackEncryption(keyId) };
        }

        #endregion
    }
}