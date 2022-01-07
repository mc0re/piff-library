namespace PiffLibrary
{
    [BoxName("tfhd")]
    internal class PiffTrackFragmentHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Which track ID this data corresponds to.
        /// </summary>
        public int TrackId { get; set; }


        /// <summary>
        /// Identical to the chunk offset in <see cref="PiffChunkOffset"/>.
        /// </summary>
        [PiffDataFormat(nameof(FlagsHaveBaseOffset))]
        public long BaseDataOffset { get; set; }


        [PiffDataFormat(nameof(FlagsHaveIndex))]
        public int SampleDescriptionIndex { get; set; }


        [PiffDataFormat(nameof(FlagsHaveDuration))]
        public int DefaultSampleDuration { get; set; }


        [PiffDataFormat(nameof(FlagsHaveSize))]
        public int DefaultSampleSize { get; set; }


        [PiffDataFormat(nameof(FlagsHaveFlags))]
        public int DefaultSampleFlags { get; set; }

        #endregion


        #region Format API

        /// <summary>
        /// 0x01 - <see cref="BaseDataOffset"/> is present
        /// 0x02 - <see cref="SampleDescriptionIndex"/> is present
        /// 0x08 - <see cref="DefaultSampleDuration"/> is present
        /// 0x10 - <see cref="DefaultSampleSize"/> is present
        /// 0x20 - <see cref="DefaultSampleFlags"/> is present
        /// 0x10000 - Duration is empty (there are no samples)
        /// 0x20000 - Default base is "moof"
        /// </summary>
        public PiffDataFormats FlagsHaveBaseOffset() =>
            (Flags & 1) != 0 ? PiffDataFormats.Int64 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveIndex() =>
            (Flags & 2) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveDuration() =>
            (Flags & 8) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveSize() =>
            (Flags & 0x10) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveFlags() =>
            (Flags & 0x20) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;

        #endregion
    }
}