namespace PiffLibrary
{

    [BoxName("url ")]
    internal class PiffDataEntryUrl : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit time and duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; set; } = 0;


        /// <summary>
        /// Whether the data is in the same file as this box.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 1;


        /// <summary>
        /// URL if <see cref="Flags"/> does not have bit 0.
        /// </summary>
        [PiffDataFormat(nameof(GetUrlFormat))]
        public string Url { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats GetUrlFormat()
        {
            return (Flags & 1) == 0 ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;
        }

        #endregion
    }
}