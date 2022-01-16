namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Associates he source file in <see cref="PiffFilePartitionBox"/>
    /// with a FEC reservoir.
    /// </summary>
    [BoxName("fecr")]
    public sealed class PiffFecReservoirBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Matching the numner of blocks in the corresponding <see cref="PiffFilePartitionBox"/>.
        /// </summary>
        [PiffDataFormat(nameof(GetCountFormat))]
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffFecReservoirItem[] Entries { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetCountFormat() =>
            Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
