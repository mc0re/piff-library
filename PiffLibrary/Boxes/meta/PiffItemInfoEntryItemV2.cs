namespace PiffLibrary.Boxes
{
    public sealed class PiffItemInfoEntryItemV2
    {
        #region Fields

        private readonly PiffItemInfoEntryBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// 0 for primary resource or an ID.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public uint ItemId { get; set; }


        /// <summary>
        /// 0 - unprotected item
        /// * - index into <see cref="PiffItemProtectionBox"/>.
        /// </summary>
        public ushort ItemProtectionIndex { get; set; }


        /// <summary>
        /// Item type indicator.
        /// </summary>
        [PiffStringLength(4)]
        public string ItemType { get; set; }


        /// <summary>
        /// Symbolic name of the item, e.g. source file for transmissions.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string ItemName { get; set; }


        /// <summary>
        /// MIME type. If the content is encoded, the original MIME type.
        /// </summary>
        [PiffDataFormat(nameof(UseMimeString))]
        public string ContentType { get; set; }


        /// <summary>
        /// Type of encoding applied, e.g. "gzip", "compress", "deflate".
        /// Same as Content-Encoding in HTTP/1.1.
        /// Empty string - no encoding.
        /// </summary>
        [PiffDataFormat(nameof(UseMimeString))]
        public string ContentEncoding { get; set; }


        /// <summary>
        /// Absolute URI used as a type indicator.
        /// </summary>
        [PiffDataFormat(nameof(UseUriString))]
        public string ItemUriType { get; set; }

        #endregion


        #region Init and clean-up

        public PiffItemInfoEntryItemV2(PiffItemInfoEntryBox parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetIdFormat() =>
            mParent.Version == 2 ? PiffDataFormats.UInt16 :
            mParent.Version == 3 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Skip;


        private PiffDataFormats UseMimeString() =>
            ItemType == "mime" ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;


        private PiffDataFormats UseUriString() =>
            ItemType == "uri " ? PiffDataFormats.AsciiZero : PiffDataFormats.Skip;

        #endregion
    }
}
