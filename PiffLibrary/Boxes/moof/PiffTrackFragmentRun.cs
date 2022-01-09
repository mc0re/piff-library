namespace PiffLibrary
{
    /// <summary>
    /// Multiple boxes per track.
    /// If <see cref="PiffTrackFragmentHeader"/> flags indicate the duration is empty,
    /// no run boxes mey be present.
    /// </summary>
    [BoxName("trun")]
    internal class PiffTrackFragmentRun : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of samples in this run.
        /// </summary>
        public uint SampleCount { get; set; }
        

        /// <summary>
        /// Added to <see cref="PiffTrackFragmentHeader.BaseDataOffset"/>.
        /// </summary>
        [PiffDataFormat(nameof(FlagsHaveDataOffset))]
        public int DataOffset { get; set; }
        

        [PiffDataFormat(nameof(FlagsHaveFirstFlags))]
        public uint FirstSampleFlags { get; set; }


        [PiffArraySize(nameof(SampleCount))]
        public PiffTrackFragmentRunSample[] Samples { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats FlagsHaveDataOffset() =>
            (Flags & 1) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveFirstFlags() =>
            (Flags & 4) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}