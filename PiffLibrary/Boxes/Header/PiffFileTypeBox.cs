using System;


namespace PiffLibrary.Boxes
{
    [BoxName("ftyp")]
    public sealed class PiffFileTypeBox : PiffBoxBase
    {
        #region Properties

        [PiffStringLength(4)]
        public string MajorBrand { get; set; }


        public uint MinorVersion { get; set; }


        [PiffStringLength(4)]
        public string[] CompatibleBrands { get; set; } = Array.Empty<string>();

        #endregion
    }
}