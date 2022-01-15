namespace PiffLibrary.Boxes
{
    [BoxName("iinf")]
    [ChildType(typeof(PiffItemInfoEntryBox))]
    public sealed class PiffItemInfoBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of <see cref="PiffItemInfoEntryBox"/> child items.
        /// Must be sorted in increasing item ID.
        /// </summary>
        [PiffDataFormat(nameof(GetCountFormat))]
        public uint Count { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetCountFormat() =>
            Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
