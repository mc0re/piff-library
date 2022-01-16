namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Relation between two meta boxes at the same level.
    /// </summary>
    [BoxName("mere")]
    public sealed class PiffMetaboxRelationBox : PiffFullBoxBase
    {
        /// <summary>
        /// Refers to <see cref="PiffHandlerTypeBox.HandlerType"/>
        /// of the first <see cref="PiffMetadataBox"/>.
        /// </summary>
        [PiffStringLength(4)]
        public string FirstHandlerType { get; set; }


        /// <summary>
        /// Refers to <see cref="PiffHandlerTypeBox.HandlerType"/>
        /// of the second <see cref="PiffMetadataBox"/>.
        /// </summary>
        [PiffStringLength(4)]
        public string SecondHandlerType { get; set; }


        /// <summary>
        /// Possible values:
        /// - 1 - Unknown
        /// - 2 - Unrelated
        /// - 3 - Complementary (disjoint)
        /// - 4 - Overlap (neither is a subset of the other)
        /// - 5 - First is preferred, the second is a subset or a weaker version
        /// - 6 - Equivalent (e.g. same data for two systems)
        /// </summary>
        public byte Relation { get; set; }
    }
}
