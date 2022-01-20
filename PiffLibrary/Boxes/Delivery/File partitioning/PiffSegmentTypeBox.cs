namespace PiffLibrary.Boxes
{
    /// <summary>
    /// For files stored in several segments.
    /// Must be the first box in the segment.
    /// </summary>
    [BoxName("styp")]
    public sealed class PiffSegmentTypeBox : PiffBoxBase
    {
        #region Properties

        [PiffStringLength(4)]
        public string MajorBrand { get; set; }


        public uint MinorVersion { get; set; }


        [PiffStringLength(4)]
        public string[] CompatibleBrands { get; set; }

        #endregion
    }
}
