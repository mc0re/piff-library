namespace PiffLibrary.Boxes
{
    [BoxName("payt")]
    public sealed class PiffHintPayloadIdBox : PiffBoxBase
    {
        /// <summary>
        /// Payload ID used in RTP packets.
        /// </summary>
        public uint PayloadId { get; set; }


        public byte Count { get; set; }


        [PiffStringLength(nameof(Count))]
        [PiffDataFormat(PiffDataFormats.Ascii)]
        public string RtpMap { get; set; }
    }
}
