using PiffLibrary.Boxes;

namespace PiffLibrary
{
    /// <summary>
    /// Ignore this.
    /// </summary>
    [BoxName("skip")]
    internal sealed class PiffSkipBox : PiffBoxBase
    {
        public byte[] Data { get; set; }
    }
}
