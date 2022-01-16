namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Info used in <see cref="PiffItemInfoBox"/>.
    /// </summary>
    [BoxName("ipro")]
    [ChildType(typeof(PiffProtectionSchemeInformationBox))]
    public sealed class PiffItemProtectionBox : PiffFullBoxBase
    {
        /// <summary>
        /// The number of <see cref="PiffProtectionSchemeInformationBox"/> children.
        /// </summary>
        public ushort Count { get; set; }
    }
}
