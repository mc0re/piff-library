namespace PiffLibrary.Boxes
{
    [BoxName("cslg")]
    public sealed class PiffCompositionToDecodeBox : PiffFullBoxBase
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

        private PiffDataFormats GetOffsetFormat() =>
            Version == 0 ? PiffDataFormats.Int32 : PiffDataFormats.Int64;

        #endregion
    }
}