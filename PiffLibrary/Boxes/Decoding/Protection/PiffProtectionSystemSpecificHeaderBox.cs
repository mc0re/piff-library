namespace PiffLibrary.Boxes
{
    [BoxName("pssh")]
    public sealed class PiffProtectionSystemSpecificHeaderBox : PiffFullBoxBase
    {
        public PiffProtectionInfo Data { get; set; }
    }
}