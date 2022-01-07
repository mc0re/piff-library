namespace PiffLibrary
{
    [BoxName("cslg")]
    internal class PiffCompositionToDecode : PiffFullBoxBase
    {
        #region Properties

        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long CompositionToDtsShift { get; set; }


        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long LeastDecodeToDisplayDelta { get; set; }


        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long GreatestDecodeToDisplayDelta { get; set; }


        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long CompositionStartTime { get; set; }


        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long CompositionEndTime { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats GetOffsetFormat() =>
            Version == 0 ? PiffDataFormats.Int32 : PiffDataFormats.Int64;

        #endregion
    }
}