namespace PiffLibrary
{
    /// <summary>
    /// Levels give dependencis. Samples on level N
    /// may depend on samples on levels less than N
    /// and shall not depend on any samples on levels greater then N.
    /// </summary>
    [BoxName("leva")]
    internal class PiffLevelAssignment : PiffFullBoxBase
    {
        /// <summary>
        /// The number of specified levels.
        /// </summary>
        public byte Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffLevelAssignmentItem[] Assignments { get; set; }
    }

    public class PiffLevelAssignmentItem
    {
        #region Properties

        /// <summary>
        /// Track being described.
        /// </summary>
        public uint TrackId { get; set; }


        /// <summary>
        /// Left-most bit, when set, means the <see cref="PiffMediaData"/>
        /// shall be padded with 0.
        /// 
        /// Next 7 bits are used for the type.
        /// Specifies how the assignment is done.
        /// 
        /// 0 - by sample groups (description indices)
        /// 1 - by parameterized sample groups
        /// 2, 3 - by tracks
        /// 4 - by sub-tracks
        /// </summary>
        public byte AssignmentType { get; set; }


        /// <summary>
        /// Level N contains samples found in "sgpd" box
        /// having the same grouping type and parameter (if present),
        /// in description entry N.
        /// </summary>
        [PiffDataFormat(nameof(UseGroupType))]
        public uint GroupingType { get; set; }


        /// <summary>
        /// Level N contains samples found in "sgpd" box
        /// having the same grouping type and parameter (if present),
        /// in description entry N.
        /// </summary>
        [PiffDataFormat(nameof(UseGroupTypeParam))]
        public uint GroupingTypeParameter { get; set; }


        /// <summary>
        /// Sub-track ID within loop entry N is mapped to level N.
        /// </summary>
        [PiffDataFormat(nameof(UseSubTrackId))]
        public uint SubTrackId { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats UseGroupType() =>
            (AssignmentType & 0x7F) == 0 || (AssignmentType & 0x7F) == 1 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats UseGroupTypeParam() =>
            (AssignmentType & 0x7F) == 1 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats UseSubTrackId() =>
            (AssignmentType & 0x7F) == 4 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}