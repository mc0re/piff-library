namespace PiffLibrary.Boxes
{
    [BoxName("url ")]
    public sealed class PiffDataEntryUrlBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// URL if <see cref="Flags"/> does not have bit 0.
        /// </summary>
        [PiffDataFormat(nameof(GetStringFormat))]
        public string Location { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetStringFormat() =>
            (Flags & 1) == 0 ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;

        #endregion
    }
}