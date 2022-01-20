namespace PiffLibrary.Boxes
{
    [BoxName("tpay")]
    public sealed class PiffHintBytesSentBox : PiffBoxBase
    {
        /// <summary>
        /// Total bytes sent, excluding RTP headers.
        /// </summary>
        public uint BytesSent { get; set; }
    }
}
