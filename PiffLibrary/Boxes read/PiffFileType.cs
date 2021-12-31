namespace PiffLibrary
{
    [BoxName("ftyp")]
    internal class PiffFileType : PiffBoxBase
    {
        [PiffDataLength(4)]
        public string MajorBrand { get; set; } = "isml";


        public int MinorVersion { get; set; } = 1;


        [PiffDataLength(4)]
        public string[] CompatibleBrands { get; set; } = { "piff", "iso2" };
    }
}