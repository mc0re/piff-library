namespace PiffLibrary
{
    [BoxName("ftyp")]
    internal class PiffFileType
    {
        public string MajorBrand { get; } = "isml";

        public int MinorVersion { get; } = 1;

        public string[] CompatibleBrands { get; } = { "piff", "iso2" };
    }
}