namespace PiffLibrary.Boxes
{
    public class PiffItemInfoEntryItemV1
    {
        #region Fields

        private readonly PiffItemInfoEntryBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// 0 for primary resource or an ID.
        /// </summary>
        public ushort ItemId { get; set; }


        /// <summary>
        /// 0 - unprotected item
        /// * - index into <see cref="PiffItemProtectionBox"/>.
        /// </summary>
        public ushort ItemProtectionIndex { get; set; }


        /// <summary>
        /// Symbolic name of the item, e.g. source file for transmissions.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string ItemName { get; set; }


        /// <summary>
        /// MIME type. If the content is encoded, the original MIME type.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string ContentType { get; set; }


        /// <summary>
        /// Type of encoding applied, e.g. "gzip", "compress", "deflate".
        /// Same as Content-Encoding in HTTP/1.1.
        /// Empty string - no encoding.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string ContentEncoding { get; set; }


        [PiffDataFormat(nameof(UseExtension))]
        public PiffItemInfoEntryExtension Extension { get; set; }

        #endregion


        #region Init and clean-up

        public PiffItemInfoEntryItemV1(PiffItemInfoEntryBox parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats UseExtension() =>
            mParent.Version == 1 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}
