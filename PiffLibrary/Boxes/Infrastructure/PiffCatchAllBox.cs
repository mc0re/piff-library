namespace PiffLibrary.Boxes
{
    /// <summary>
    /// All unrecognized boxes are given this type.
    /// </summary>
    [BoxName("unknown")]
    public sealed class PiffCatchAllBox : PiffBoxBase
    {
        public byte[] Data { get; set; }


        [PiffDataFormat(PiffDataFormats.Skip)]
        public string BoxType { get; set; }


        public override string ToString() => $"{BoxType} (unknown)";
    }
}
