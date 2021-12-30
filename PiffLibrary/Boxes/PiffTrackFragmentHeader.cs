namespace PiffLibrary
{
    [BoxName("tfhd")]
    public class PiffTrackFragmentHeader
    {
        #region Properties

        public byte Version { get; internal set; } = 0;


        /// <summary>
        /// 0x01 - <see cref="BaseDataOffset"/> is present
        /// 0x02 - <see cref="SampleDescriptionIndex"/> is present
        /// 0x08 - <see cref="DefaultSampleDuration"/> is present
        /// 0x10 - <see cref="DefaultSampleSize"/> is present
        /// 0x20 - <see cref="DefaultSampleFlage"/> is present
        /// 0x10000 - Duration is empty (there are no samples)
        /// 0x20000 - Default base is "moof"
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; internal set; } = 0;


        public int TrackId { get; internal set; }


        /// <summary>
        /// Identical to the chunk offset in <see cref="PiffChunkOffset"/>.
        /// </summary>
        public long BaseDataOffset { get; internal set; }


        public int SampleDescriptionIndex { get; internal set; }


        public int DefaultSampleDuration { get; internal set; }


        public int DefaultSampleSize { get; internal set; }


        public int DefaultSampleFlage { get; internal set; }

        #endregion
    }
}