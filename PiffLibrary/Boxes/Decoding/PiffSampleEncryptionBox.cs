namespace PiffLibrary.Boxes
{
    [BoxName("senc")]
    public sealed class PiffSampleEncryptionBox : PiffFullBoxBase
    {
        public PiffSampleEncryption Data { get; set; }
    }
}