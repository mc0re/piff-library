namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Ignore this box.
    /// </summary>
    [BoxName("free")]
    [BoxName("skip")]
    public sealed class PiffSkipBox : PiffBoxBase
    {
        public byte[] Data { get; set; }
    }
}
