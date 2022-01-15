namespace PiffLibrary.Boxes
{
    /// <summary>
    /// A directory of resources in this or other files.
    /// </summary>
    [BoxName("iloc")]
    public sealed class PiffItemLocationBox : PiffFullBoxBase
    {
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte OffsetSize { get; set; }


        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte LengthSize { get; set; }
    }
}
