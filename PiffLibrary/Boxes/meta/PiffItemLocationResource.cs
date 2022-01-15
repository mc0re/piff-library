namespace PiffLibrary.Boxes
{
    public sealed class PiffItemLocationResource
    {
        #region Properties

        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffItemLocationBox Parent { get; }


        /// <summary>
        /// Arbitrary integer identifying the resource.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public uint ItemId { get; set; }


        /// <summary>
        /// First 12 bits are reserved 0, only 4 bits are the value.
        /// 
        /// - 0 "file offset" - <see cref="DataReferenceIndex"/> are absolute file offsets
        /// - 1 "idat offset" - offsets into "idat" bos in the same <see cref="PiffMetadataBox"/>
        /// - 2 "item offset" - item offsets as given by <see cref="ExtentItems"/>
        /// </summary>
        [PiffDataFormat(nameof(GetMethodFormat))]
        public ushort ConstructionMethod { get; set; }


        /// <summary>
        /// 0 = reference into this file,
        /// or an index into <see cref="PiffDataInformationBox"/>.
        /// </summary>
        public ushort DataReferenceIndex { get; set; }


        /// <summary>
        /// Base value for offset calculations. If missing, 0 implied.
        /// </summary>
        [PiffDataFormat(nameof(GetBaseOffsetFormat))]
        public ulong BaseOffset { get; set; }


        /// <summary>
        /// The number of extents in this resource.
        /// An extent is a contiguous subset of the resource bytes.
        /// Must be at least 1.
        /// If only one extent is used:
        /// - missing offset implies 0
        /// - missing length or 0 length implies the entire resource length.
        /// </summary>
        public ushort ExtentCount { get; set; }


        [PiffArraySize(nameof(ExtentCount))]
        public PiffItemLocationExtent[] ExtentItems { get; set; }

        #endregion


        #region Init and clean-up

        public PiffItemLocationResource(PiffItemLocationBox parent)
        {
            Parent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetIdFormat() =>
            Parent.Version == 2 ? PiffDataFormats.UInt32 :
            Parent.Version < 2 ? PiffDataFormats.UInt16 :
            PiffDataFormats.Skip;


        private PiffDataFormats GetMethodFormat() =>
            Parent.Version == 1 || Parent.Version == 2 ? PiffDataFormats.UInt16 : PiffDataFormats.Skip;


        private PiffDataFormats GetBaseOffsetFormat() =>
            Parent.BaseOffsetSize == 8 ? PiffDataFormats.UInt64 :
            Parent.BaseOffsetSize == 4 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Skip;

        #endregion
    }
}
