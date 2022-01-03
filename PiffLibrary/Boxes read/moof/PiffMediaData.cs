namespace PiffLibrary
{
    /// <summary>
    /// Raw video or audio data.
    /// </summary>
    [BoxName("mdat")]
    internal class PiffMediaData : PiffBoxBase
    {
        public byte[] Data { get; set; }
    }
}