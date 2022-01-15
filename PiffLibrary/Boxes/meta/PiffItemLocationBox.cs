namespace PiffLibrary.Boxes
{
    /// <summary>
    /// A directory of resources in this or other files.
    /// </summary>
    [BoxName("iloc")]
    public sealed class PiffItemLocationBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Length of <see cref="PiffItemLocationExtent.Offset"/> in bytes.
        /// Allowed values: 0, 4, 8.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte OffsetSize { get; set; }


        /// <summary>
        /// Length of <see cref="PiffItemLocationExtent.Length"/> in bytes.
        /// Allowed values: 0, 4, 8.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte LengthSize { get; set; }


        /// <summary>
        /// Length of <see cref="PiffItemLocationResource.BaseOffset"/> in bytes.
        /// Allowed values: 0, 4, 8.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte BaseOffsetSize { get; set; }


        /// <summary>
        /// Length of <see cref="PiffItemLocationExtent.Index"/> in bytes.
        /// Allowed values: 0, 4, 8.
        /// Only valid for <see cref="Version"/> 1 or 2, else ignored.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte IndexSize { get; set; }


        [PiffDataFormat(nameof(GetCountFormat))]
        public uint ItemCount { get; set; }


        [PiffArraySize(nameof(ItemCount))]
        public PiffItemLocationResource[] Items { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetCountFormat() =>
            Version == 2 ? PiffDataFormats.UInt32 :
            Version < 2 ? PiffDataFormats.UInt16 :
            PiffDataFormats.Skip;

        #endregion
    }
}
