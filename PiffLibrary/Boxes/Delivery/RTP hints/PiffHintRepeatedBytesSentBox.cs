namespace PiffLibrary.Boxes
{
    [BoxName("drep")]
    public sealed class PiffHintRepeatedBytesSentBox : PiffBoxBase
    {
        /// <summary>
        /// Total bytes in repeated packets.
        /// </summary>
        public ulong BytesSent { get; set; }
    }
}
