namespace PiffLibrary.Boxes
{
    [BoxName("trpy")]
    public sealed class PiffHintTotalBytesSent64Box : PiffBoxBase
    {
        /// <summary>
        /// Total bytes sent, including RTP headers.
        /// </summary>
        public ulong BytesSent { get; set; }
    }
}
