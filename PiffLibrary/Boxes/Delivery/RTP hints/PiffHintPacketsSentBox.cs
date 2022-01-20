namespace PiffLibrary.Boxes
{
    [BoxName("npck")]
    public sealed class PiffHintPacketsSentBox : PiffBoxBase
    {
        public uint PacketsSent { get; set; }
    }
}
