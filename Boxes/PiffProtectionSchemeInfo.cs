using System;

namespace PiffLibrary
{
    [BoxName("schi")]
    internal class PiffProtectionSchemeInfo
    {
        #region Properties

        public PiffProtectionTrackEncryption Data { get; }

        #endregion


        #region Init and clean-up

        public PiffProtectionSchemeInfo(Guid keyId)
        {
            Data = new PiffProtectionTrackEncryption(keyId);
        }

        #endregion
    }
}