using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("padb")]
    internal class PiffPaddingBits : PiffFullBoxBase
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

        [PiffDataFormat(PiffDataFormats.Skip)]
        public uint NofBytes => (SampleCount + 1) / 2;

        #endregion
    }
}