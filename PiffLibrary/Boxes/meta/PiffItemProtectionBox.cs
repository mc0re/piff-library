namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Info used in <see cref="PiffItemInfoBox"/>.
    /// </summary>
    [BoxName("ipro")]
    public sealed class PiffItemProtectionBox : PiffFullBoxBase
    {
        /// <summary>
        /// The number of <see cref="PiffProtectionSchemeInfoBox"/> children.
        /// </summary>
        public ushort Count { get; set; }
    }
}
