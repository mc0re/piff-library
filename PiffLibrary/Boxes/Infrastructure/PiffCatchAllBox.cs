namespace PiffLibrary.Boxes
{
    /// <summary>
    /// All unrecognized boxes are given this type.
    /// </summary>
    [BoxName("unknown")]
    public sealed class PiffCatchAllBox : PiffBoxBase
    {
        public byte[] Data { get; set; }


        public override string ToString() => $"{base.ToString()} (unknown)";
    }
}
