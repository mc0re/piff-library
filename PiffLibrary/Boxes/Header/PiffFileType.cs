namespace PiffLibrary
{
    [BoxName("ftyp")]
    internal class PiffFileType : PiffBoxBase
    {
        [PiffStringLength(4)]
        public string MajorBrand { get; set; } = "isml";


        public uint MinorVersion { get; set; } = 1;


        [PiffStringLength(4)]
        public string[] CompatibleBrands { get; set; } = { "piff", "iso2" };
    }
}