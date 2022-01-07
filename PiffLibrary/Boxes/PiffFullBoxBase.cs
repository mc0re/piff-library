namespace PiffLibrary
{
    internal abstract class PiffFullBoxBase : PiffBoxBase
    {
        [BeforeDescendants]
        public byte Version { get; set; }


        [PiffDataFormat(PiffDataFormats.Int24)]
        [BeforeDescendants]
        public int Flags { get; set; }
    }
}