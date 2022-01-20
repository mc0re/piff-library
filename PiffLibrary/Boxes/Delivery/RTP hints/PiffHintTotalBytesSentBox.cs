namespace PiffLibrary.Boxes
{
    [BoxName("totl")]
    public sealed class PiffHintTotalBytesSentBox : PiffBoxBase
    {
        /// <summary>
        /// Total bytes sent, including RTP headers.
        /// </summary>
        public uint BytesSent { get; set; }
    }
}
