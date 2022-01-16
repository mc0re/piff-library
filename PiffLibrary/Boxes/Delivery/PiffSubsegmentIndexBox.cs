namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Mapping from levels to byte rages.
    /// </summary>
    [BoxName("ssix")]
    public sealed class PiffSubsegmentIndexBox : PiffFullBoxBase
    {
        public uint SubsegmentCount { get; set; }


        [PiffArraySize(nameof(SubsegmentCount))]
        public PiffSubsegmentIndexItem[] Subsegments { get; set; }
    }
}
