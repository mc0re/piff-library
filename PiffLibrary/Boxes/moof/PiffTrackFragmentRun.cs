namespace PiffLibrary
{
    [BoxName("trun")]
    internal class PiffTrackFragmentRun : PiffFullBoxBase
    {
        #region Properties

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