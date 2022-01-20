namespace PiffLibrary.Boxes
{
    [BoxName("pmax")]
    public sealed class PiffHintLargestPacketBox : PiffBoxBase
    {
        /// <summary>
        /// Largest packet sent, including RTP header.
        /// </summary>
        public uint Bytes { get; set; }
    }
}
