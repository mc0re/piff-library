namespace PiffLibrary.Boxes
{
    public sealed class PiffSampleEncryptionItem
    {
        #region Fields

        private readonly PiffSampleEncryption mParent;

        #endregion


        #region Properties

        [PiffArraySize(nameof(InitVectorSize))]
        public byte[] InitVector { get; set; }


        [PiffDataFormat(nameof(UseSubSample))]
        public ushort SubSampleCount { get; set; }


        [PiffArraySize(nameof(SubSampleCount))]
        public PiffSampleEncryptionSubSample[] SubSamples { get; set; }

        #endregion


        #region Init and clean-up

        public PiffSampleEncryptionItem(PiffSampleEncryption parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        /// <summary>
        /// The number of bytes in the initialization vector.
        /// If not specified (Flags is ___0), the default value seems to be 8.
        /// </summary>
        private int InitVectorSize => mParent.Algorithm?.InitVectorSize ?? 8;


        private PiffDataFormats UseSubSample() =>
            (mParent.Parent.Flags & 2) != 0 ? PiffDataFormats.UInt16 : PiffDataFormats.Skip;

        #endregion
    }
}