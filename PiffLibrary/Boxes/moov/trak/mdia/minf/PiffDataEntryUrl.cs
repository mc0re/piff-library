namespace PiffLibrary
{
    [BoxName("url ")]
    internal class PiffDataEntryUrl : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// URL if <see cref="Flags"/> does not have bit 0.
        /// </summary>
        [PiffDataFormat(nameof(GetStringFormat))]
        public string Location { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats GetStringFormat()
        {
            return (Flags & 1) == 0 ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;
        }

        #endregion
    }
}