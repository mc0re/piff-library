namespace PiffLibrary.Boxes
{
    [BoxName("stsg")]
    public sealed class PiffSubTrackSampleGroupBox : PiffFullBoxBase
    {
        /// <summary>
        /// Same value as in the corresponding <see cref="PiffSampleToGroupBox"/>
        /// and <see cref="PiffSampleDescriptionBox"/>.
        /// </summary>
        public uint GroupingType { get; set; }


        public ushort Count { get; set; }


        /// <summary>
        /// Index of the sample group entry.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        public uint[] DescriptionIndices { get; set; }
    }
}
