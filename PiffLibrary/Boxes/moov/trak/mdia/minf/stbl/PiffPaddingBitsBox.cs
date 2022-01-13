namespace PiffLibrary.Boxes
{
    [BoxName("padb")]
    public sealed class PiffPaddingBitsBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of samples in this track.
        /// </summary>
        public uint SampleCount { get; set; }


        /// <summary>
        /// Two entries in each byte:
        /// 1 bit - reserved
        /// 3 bits - pad, the number of bits at the end of a sample
        /// </summary>
        [PiffArraySize(nameof(NofBytes))]
        public byte[] Padding { get; set; }

        #endregion


        #region Format API

        private uint NofBytes => (SampleCount + 1) / 2;

        #endregion
    }
}