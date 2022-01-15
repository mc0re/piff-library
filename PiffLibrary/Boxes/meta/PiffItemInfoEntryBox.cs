namespace PiffLibrary.Boxes
{
    [BoxName("infe")]
    public sealed class PiffItemInfoEntryBox : PiffFullBoxBase
    {
        #region Properties

        [PiffDataFormat(nameof(UseV1Header))]
        public PiffItemInfoEntryItemV1 HeaderV1 { get; set; }


        [PiffDataFormat(nameof(UseV2Header))]
        public PiffItemInfoEntryItemV2 HeaderV2 { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats UseV1Header() =>
            Version == 0 || Version == 1 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats UseV2Header() =>
            Version >= 2 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}
