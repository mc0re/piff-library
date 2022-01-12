namespace PiffLibrary.Boxes
{
    [BoxName("stz2")]
    public sealed class PiffCompactSampleSizeBox : PiffFullBoxBase
    {
        #region Properties

        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Reserved { get; }


        /// <summary>
        /// Size of a single entry, in bits; can be 4, 8, or 16.
        /// </summary>
        public byte FieldSize { get; set; }


        /// <summary>
        /// The number of samples in the track.
        /// </summary>
        public uint SampleCount { get; set; }


        /// <summary>
        /// Individual sizes for each sample in the track.
        /// This is compacted array with potentially several entries in each byte.
        /// </summary>
        [PiffArraySize(nameof(NumberOfBytes))]
        public byte[] EntrySizes { get; set; }

        #endregion


        #region Format API

        /// <summary>
        /// Round up to the next byte.
        /// </summary>
        private uint NumberOfBytes => (SampleCount * FieldSize + 7) / 8;

        #endregion
    }
}