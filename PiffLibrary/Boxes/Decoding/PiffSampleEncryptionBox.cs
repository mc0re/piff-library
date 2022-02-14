namespace PiffLibrary.Boxes
{
    [BoxName("senc")]
    public sealed class PiffSampleEncryptionBox : PiffFullBoxBase
    {
        /// <summary>
        /// Same data can be stored as this box or as an extension box.
        /// </summary>
        public PiffSampleEncryption Data { get; set; }
    }
}