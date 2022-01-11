namespace PiffLibrary
{
    [BoxName("assp")]
    internal class PiffAlternativeStartupSequence : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// No value of sample offset in <see cref="PiffSampleToGroup"/>
        /// shall be smaller than this value.
        /// </summary>
        [PiffDataFormat(nameof(UseV0Offset))]
        public int MinStartupOffset { get; set; }


        [PiffDataFormat(nameof(UseV1OffsetCount))]
        public int NofEntries { get; set; }


        [PiffDataFormat(nameof(UseV1OffsetArray))]
        [PiffArraySize(nameof(NofEntries))]
        public PiffAlternativeStartupSequenceItem[] MinStartupOffsets { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats UseV0Offset() =>
            Version == 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats UseV1OffsetCount() =>
            Version == 1 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats UseV1OffsetArray() =>
            Version == 1 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}