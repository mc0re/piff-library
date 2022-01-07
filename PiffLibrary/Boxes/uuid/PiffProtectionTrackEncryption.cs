using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines encryption parameters for all samples in a track,
    /// unless overwritten by <see cref="PiffSampleEncryption"/> extension box.
    /// </summary>
    internal class PiffProtectionTrackEncryption
    {
        #region Constants

        public static readonly Guid BoxId = Guid.Parse("8974dbce-7be7-4c51-84f9-7148f9882554");

        #endregion


        #region Properties

        /// <summary>
        /// 0 - not encrypted
        /// 1 - AES 128-bit in CTR mode
        /// 2 - AES 128-bit in CBC mode
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int DefaultAlgorithmId { get; set; } = 1;


        /// <summary>
        /// 8 for AES-CTR
        /// 16 for AES-CTR and AES-CBC
        /// </summary>
        public byte DefaultIvSize { get; set; } = 8;


        public Guid ContentKeyId { get; set; }

        #endregion
    }
}