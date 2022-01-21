namespace PiffLibrary.Boxes
{
    [BoxName("extr")]
    public sealed class PiffExtraDataBox : PiffBoxBase
    {
        public PiffFecInformationBox FecInfo { get; set; }


        public byte[] ExtraData { get; set; }
    }
}
