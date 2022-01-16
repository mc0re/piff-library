namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Identifies a source files and partitions it into source blocks and symbols.
    /// </summary>
    [BoxName("fpar")]
    public sealed class PiffFilePartitionBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Reference to <see cref="PiffItemLocationBox"/> (<see cref="PiffItemLocationResource.ItemId"/>).
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public uint ItemId { get; set; }

        public ushort PacketPayloadSize { get; set; }

        public byte Reserved { get; }


        /// <summary>
        /// See RFC 5052.
        /// 0 - null-FEC (RFC 3695)
        /// 1 - MBMS FEC (3GPP TS 26.346)
        /// ...
        /// </summary>
        public byte FecEncodingId { get; set; }


        /// <summary>
        /// A more specific identification of the FEC encoder being used.
        /// </summary>
        public ushort FecInstanceId { get; set; }


        /// <summary>
        /// The maximum number of source symbols per source block.
        /// </summary>
        public ushort MaxSourceBlockLength { get; set; }


        /// <summary>
        /// Size of a single encoding item in bytes.
        /// The last item may be shorter than this.
        /// </summary>
        public ushort EncodingSymbolLength { get; set; }


        /// <summary>
        /// The maximum number of encoding sybols that can be generated for a source block.
        /// </summary>
        public ushort MaxNumberOfSymbols { get; set; }


        /// <summary>
        /// Base64-encoded string, depending on <see cref="FecEncodingId"/>.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string SchemeSpecificInfo { get; set; }


        [PiffDataFormat(nameof(GetCountFormat))]
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffFilePartitionItem[] Entries { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetIdFormat() =>
            Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;


        private PiffDataFormats GetCountFormat() =>
            Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
