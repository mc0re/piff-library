namespace PiffLibrary.Boxes
{
    [BoxName("fdpa")]
    public sealed class PiffFdPacketBox : PiffBoxBase
    {
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte SenderCurrentTimePresent { get; set; }

        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte ExpectedResidualTimePresent { get; set; }

        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte SessionCloseBit { get; set; }

        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte ObjectCloseBit { get; set; }

        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte Reserved { get; set; }

        public ushort TransportObjectIdentifier { get; set; }

        public ushort ExtensionCount { get; set; }

        [PiffArraySize(nameof(ExtensionCount))]
        public PiffLctHeaderExtensionItem[] Extensions { get; set; }
        
        public ushort ConstructorCount { get; set; }

        [PiffArraySize(nameof(ConstructorCount))]
        public PiffLctConstructorItem[] Constructors { get; set; }
    }
}
