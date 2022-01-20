namespace PiffLibrary.Boxes
{
    [BoxName("tpyl")]
    public sealed class PiffHintBytesSent64Box : PiffBoxBase
    {
        /// <summary>
        /// Total bytes sent, excluding RTP headers.
        /// </summary>
        public ulong BytesSent { get; set; }
    }
}
