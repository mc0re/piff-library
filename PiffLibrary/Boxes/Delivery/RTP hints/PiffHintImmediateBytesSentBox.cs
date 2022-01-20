namespace PiffLibrary.Boxes
{
    [BoxName("dimm")]
    public sealed class PiffHintImmediateBytesSentBox : PiffBoxBase
    {
        /// <summary>
        /// Total bytes sent immediate mode.
        /// </summary>
        public ulong BytesSent { get; set; }
    }
}
