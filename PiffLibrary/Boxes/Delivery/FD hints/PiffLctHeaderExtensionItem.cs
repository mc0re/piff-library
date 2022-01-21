namespace PiffLibrary.Boxes
{
    public class PiffLctHeaderExtensionItem
    {
        #region Properties

        /// <summary>
        /// Extension type (HET value).
        /// </summary>
        public byte Type { get; set; }

        [PiffDataFormat(nameof(UseLength))]
        public byte Length { get; set; }

        [PiffArraySize(nameof(ContentCount))]
        public int[] Content { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats UseLength() =>
            Type <= 127 ? PiffDataFormats.UInt8 : PiffDataFormats.Skip;

        private int ContentCount =>
            Type > 127 ? 3 :
            Length > 0 ? Length * 4 - 2 :
            0;

        #endregion
    }
}
