namespace PiffLibrary.Boxes
{
    public sealed class PiffColourOnScreenColoursProfile
    {
        public ushort ColourPrimaries { get; set; }
     
        public ushort TransferCharacteristics { get; set; }
        
        public ushort MatrixCoefficients { get; set; }
        
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte FullRangeFlag { get; set; }
        
        [PiffDataFormat(PiffDataFormats.UInt7)]
        public byte Reserved { get; }
    }
}