namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Multiple boxes per track.
    /// If <see cref="PiffTrackFragmentHeaderBox"/> flags indicate the duration is empty,
    /// no run boxes mey be present.
    /// </summary>
    [BoxName("trun")]
    public sealed class PiffTrackFragmentRunBox : PiffFullBoxBase
    {
        #region Constants

        public const int FlagsDataOffsetPresent = 1;

        public const int FlagsFirstSampleFlagPresent = 4;

        public const int FlagsSampleDurationPresent = 0x100;

        public const int FlagsSampleSizePresent = 0x200;
        
        public const int FlagsSampleFlagsPresent = 0x400;
        
        public const int FlagsTimeOffsetPresent = 0x800;
        
        #endregion


        #region Properties

        /// <summary>
        /// The number of samples in this run.
        /// </summary>
        public uint SampleCount { get; set; }


        /// <summary>
        /// Offset of the actual data.
        /// Added to <see cref="PiffTrackFragmentHeaderBox.BaseDataOffset"/>.
        /// </summary>
        [PiffDataFormat(nameof(FlagsHaveDataOffset))]
        public int DataOffset { get; set; }


        [PiffDataFormat(nameof(FlagsHaveFirstFlags))]
        public uint FirstSampleFlags { get; set; }


        [PiffArraySize(nameof(SampleCount))]
        public PiffTrackFragmentRunSample[] Samples { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats FlagsHaveDataOffset() =>
            (Flags & FlagsDataOffsetPresent) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveFirstFlags() =>
            (Flags & FlagsFirstSampleFlagPresent) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}