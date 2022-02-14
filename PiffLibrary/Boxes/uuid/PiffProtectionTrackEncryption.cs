using System;


namespace PiffLibrary.Boxes
{

    /// <summary>
    /// Defines encryption parameters for all samples in a track,
    /// unless overwritten by <see cref="PiffSampleEncryption"/> extension box.
    /// </summary>
    public sealed class PiffProtectionTrackEncryption
    {
        #region Constants

        public static readonly Guid BoxId = Guid.Parse("8974dbce-7be7-4c51-84f9-7148f9882554");

        #endregion


        #region Properties

        public byte Reserved1 { get; }


        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte DefaultCryptByteBlock { get; set; }


        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte DefaultSkipByteBlock { get; set; }


        /// <summary>
        /// Default track encryption algorithm.
        /// Overriden by <see cref="PiffSampleEncryptionAlgorithm"/>.
        /// Values are from <see cref="PiffEncryptionTypes"/>.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt8)]
        public PiffEncryptionTypes DefaultAlgorithmId { get; set; } = PiffEncryptionTypes.AesCtr;


        /// <summary>
        /// Default initialization ector size.
        /// Overriden by <see cref="PiffSampleEncryptionAlgorithm"/>.
        /// 
        /// 8 for AES-CTR
        /// 16 for AES-CTR and AES-CBC
        /// </summary>
        public byte DefaultPerSampleIvSize { get; set; } = 8;


        /// <summary>
        /// Default encryption key ID.
        /// </summary>
        public Guid ContentKeyId { get; set; }


        /// <summary>
        /// Max 16.
        /// </summary>
        [PiffDataFormat(nameof(UseConstantIv))]
        public byte ConstantInitVectorSize { get; set; }


        [PiffArraySize(nameof(ConstantInitVectorSize))]
        public byte[] ConstantInitVector { get; set; }

        #endregion


        #region Format

        private PiffDataFormats UseConstantIv() =>
            DefaultPerSampleIvSize == 0 ? PiffDataFormats.UInt8 : PiffDataFormats.Skip;

        #endregion
    }
}
