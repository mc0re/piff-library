namespace PiffLibrary
{
    [BoxName("trun")]
    internal class PiffTrackFragmentRun : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        /// <summary>
        /// 0x10000 - Duration is empty (there are no samples)
        /// 0x20000 - Default base is "moof"
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        public int SampleCount { get; set; }
        

        [PiffDataFormat(nameof(FlagsHaveDataOffset))]
        public int DataOffset { get; set; }
        

        [PiffDataFormat(nameof(FlagsHaveFirstFlags))]
        public int FirstSampleFlags { get; set; }


        [PiffArraySize(nameof(SampleCount))]
        public PiffTrackFragmentRunSample[] Samples { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats FlagsHaveDataOffset() =>
            (Flags & 1) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveFirstFlags() =>
            (Flags & 4) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;

        #endregion
    }
}