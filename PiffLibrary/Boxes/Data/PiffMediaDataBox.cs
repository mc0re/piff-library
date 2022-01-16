namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Raw video or audio data.
    /// </summary>
    [BoxName("mdat")]
    public sealed class PiffMediaDataBox : PiffBoxBase
    {
        public byte[] RawData { get; set; }
    }
}