using System;

namespace PiffLibrary
{
    [BoxName("schi")]
    internal class PiffProtectionSchemeInfo : PiffBoxBase
    {
        #region Properties

        public PiffProtectionTrackEncryption Data { get; set; }

        #endregion


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
            Data = new PiffProtectionTrackEncryption(keyId);
        }

        #endregion
    }
}