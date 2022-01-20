namespace PiffLibrary.Boxes
{
    [BoxName("dmax")]
    public sealed class PiffHintLongestPacketBox : PiffBoxBase
    {
        /// <summary>
        /// Longest packet duration [milliseconds].
        /// </summary>
        public uint Time { get; set; }
    }
}
