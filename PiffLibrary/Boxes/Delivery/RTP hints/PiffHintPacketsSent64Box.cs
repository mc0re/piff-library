namespace PiffLibrary.Boxes
{
    [BoxName("nump")]
    public sealed class PiffHintPacketsSent64Box : PiffBoxBase
    {
        public ulong PacketsSent { get; set; }
    }
}
