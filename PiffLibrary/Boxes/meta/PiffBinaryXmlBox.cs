namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Direct storage of binary XML.
    /// </summary>
    [BoxName("bxml")]
    public sealed class PiffBinaryXmlBox : PiffFullBoxBase
    {
        public byte[] Data { get; set; }
    }
}
