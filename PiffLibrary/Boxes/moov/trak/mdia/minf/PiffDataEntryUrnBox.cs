namespace PiffLibrary.Boxes
{
    [BoxName("urn ")]
    public sealed class PiffDataEntryUrnBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Resource name.
        /// </summary>
        [PiffDataFormat(nameof(HasString))]
        public string Name { get; set; }


        /// <summary>
        /// Resource location.
        /// </summary>
        [PiffDataFormat(nameof(HasString))]
        public string Location { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats HasString() =>
            (Flags & 1) == 0 ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;

        #endregion
    }
}