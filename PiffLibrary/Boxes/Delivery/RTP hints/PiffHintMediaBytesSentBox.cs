namespace PiffLibrary.Boxes
{
    [BoxName("dmed")]
    public sealed class PiffHintMediaBytesSentBox : PiffBoxBase
    {
        /// <summary>
        /// Total bytes sent from media tracks.
        /// </summary>
        public ulong BytesSent { get; set; }
    }
}
