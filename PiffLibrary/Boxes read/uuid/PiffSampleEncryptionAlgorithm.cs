using System;

namespace PiffLibrary
{
    internal class PiffSampleEncryptionAlgorithm
    {
        #region Properties

        /// <summary>
        /// Identifies encryption algorithm.
        /// - 0 - not encrypted
        /// - 1 - AES-CTR 128 bit
        /// - 2 - AES-CBC 128 bit
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int AlgorithmId { get; set; }


        /// <summary>
        /// Size of the initialization vector in bytes.
        /// - 8 - for AES-CTR, then these are bytes 0..7 of the IV,
        ///        and bytes 8..15 are a block counter (starting with 0, in BE format)
        /// - 16 - for both AES-CTR and AES-CBC
        /// </summary>
        public byte InitVectorSize { get; set; }


        /// <summary>
        /// Decryption key identifier.
        /// </summary>
        public Guid KeyId { get; set; }

        #endregion
    }
}