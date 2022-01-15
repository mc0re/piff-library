namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Ignore this box.
    /// </summary>
    [BoxName("skip")]
    [BoxName("free")]
    public sealed class PiffSkipBox : PiffBoxBase
    {
        public byte[] Data { get; set; }
    }
}
