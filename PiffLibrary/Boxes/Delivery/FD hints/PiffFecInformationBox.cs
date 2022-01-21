namespace PiffLibrary.Boxes
{
    [BoxName("feci")]
    public sealed class PiffFecInformationBox : PiffBoxBase
    {
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
        /// Which source block the FD packet is generated from.
        /// </summary>
        public ushort SourceBlockNumber { get; set; }


        /// <summary>
        /// Which specific encoding symbols are carried in the FD packet.
        /// </summary>
        public ushort EncodingSymbolId { get; set; }
    }
}
