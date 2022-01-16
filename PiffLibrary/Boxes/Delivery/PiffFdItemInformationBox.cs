namespace PiffLibrary.Boxes
{
    /// <summary>
    /// File delivery information.
    /// </summary>
    [BoxName("fiin")]
    [ChildType(typeof(PiffPartitionEntryBox))]
    [ChildType(typeof(PiffFdSessionGroupBox))]
    [ChildType(typeof(PiffGroupIdToNameBox))]
    public sealed class PiffFdItemInformationBox : PiffFullBoxBase
    {
        /// <summary>
        /// The number of <see cref="PiffPartitionEntryBox"/> children.
        /// </summary>
        public ushort Count { get; set; }
    }
}
