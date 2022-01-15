namespace PiffLibrary.Boxes
{
    [BoxName("pitm")]
    public sealed class PiffPrimaryItemBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Which referenced item is the primary one.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public uint ItemId { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetIdFormat() =>
            Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
