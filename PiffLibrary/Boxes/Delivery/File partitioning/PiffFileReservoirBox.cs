namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Associate the source file from <see cref="PiffFilePartitionBox"/>
    /// with a file reservoir.
    /// </summary>
    [BoxName("fire")]
    public sealed class PiffFileReservoirBox : PiffFullBoxBase
    {
        #region Properties

        [PiffDataFormat(nameof(GetCountFormat))]
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffFileReservoirItem[] Entries { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetCountFormat() =>
            Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
