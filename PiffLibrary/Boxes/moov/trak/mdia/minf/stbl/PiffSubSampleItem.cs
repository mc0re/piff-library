namespace PiffLibrary
{
    internal class PiffSubSampleItem
    {
        #region Fields

        private readonly PiffSubSampleChunk mParent;

        #endregion


        #region Properties

        /// <summary>
        /// The size of the subsample, in bytes.
        /// </summary>
        [PiffDataFormat(nameof(GetSizeFormat))]
        public ulong Size { get; set; }


        /// <summary>
        /// Degradation priority.
        /// The higher the priority, the higher the impact on the decoded quality.
        /// </summary>
        public byte Priority { get; set; }


        /// <summary>
        /// 0 - the subsample is required for decoding the sample
        /// 1 - the subsample is not required but is an enhancement
        /// </summary>
        public byte Discardable { get; set; }


        public uint CodecParameters { get; set; }

        #endregion


        #region Init and clean-up

        public PiffSubSampleItem(PiffSubSampleChunk parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        public PiffDataFormats GetSizeFormat() =>
            mParent.Parent.Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}