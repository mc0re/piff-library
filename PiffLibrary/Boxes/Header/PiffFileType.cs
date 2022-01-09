using System;


namespace PiffLibrary
{
    [BoxName("ftyp")]
    internal class PiffFileType : PiffBoxBase
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