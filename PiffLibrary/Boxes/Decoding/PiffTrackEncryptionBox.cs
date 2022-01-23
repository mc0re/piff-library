using System;


namespace PiffLibrary.Boxes
{
    [BoxName("tenc")]
    public sealed class PiffTrackEncryptionBox : PiffFullBoxBase
    {
        #region Properties

        public byte Reserved1 { get; }

        
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte DefaultCryptByteBlock { get; set; }

        
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte DefaultSkipByteBlock { get; set; }

        public byte DefaultIsProtected { get; set; }

        public byte DefaultPerSampleIvSize { get; set; }

        public Guid DefaultKeyId { get; set; }


        /// <summary>
        /// Max 16.
        /// </summary>
        [PiffDataFormat(nameof(UseConstantIv))]
        public byte DefaultConstantIvSize { get; set; }


        [PiffArraySize(nameof(DefaultConstantIvSize))]
        public byte[] DefaultConstantIv { get; set; }

        #endregion


        #region Format

        private PiffDataFormats UseConstantIv() =>
            DefaultPerSampleIvSize == 0 ? PiffDataFormats.UInt8 : PiffDataFormats.Skip;

        #endregion
    }
}