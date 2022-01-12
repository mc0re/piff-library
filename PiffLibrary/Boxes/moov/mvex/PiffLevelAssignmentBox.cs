namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Levels give dependencis. Samples on level N
    /// may depend on samples on levels less than N
    /// and shall not depend on any samples on levels greater then N.
    /// </summary>
    [BoxName("leva")]
    public sealed class PiffLevelAssignmentBox : PiffFullBoxBase
    {
        /// <summary>
        /// The number of specified levels.
        /// </summary>
        public byte Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffLevelAssignmentItem[] Assignments { get; set; }
    }
}