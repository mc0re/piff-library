using System;

namespace PiffLibrary
{
    [BoxName("uuid")]
    internal class PiffProtectionTrackEncryption
    {
        #region Properties

        /// <summary>
        /// This GUID corresponds to the protection box.
        /// </summary>
        public Guid BoxId { get; } = Guid.Parse("8974dbce-7be7-4c51-84f9-7148f9882554");


        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        /// <summary>
        /// 0 - not encrypted
        /// 1 - AES 128-bit in CTR mode
        /// 2 - AES 128-bit in CBC mode
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int DefaultAlgorithmId { get; } = 1;


        /// <summary>
        /// 8 for AES-CTR
        /// 16 for AES-CTR and AES-CBC
        /// </summary>
        public byte DefaultIvSize { get; } = 8;


        public Guid ContentKeyId { get; }

        #endregion


        #region Init and clean-up

        public PiffProtectionTrackEncryption(Guid keyId)
        {
            ContentKeyId = keyId;
        }

        #endregion
    }
}