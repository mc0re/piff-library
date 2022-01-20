namespace PiffLibrary.Boxes
{
    public sealed class PiffSubsegmentIndexItem
    {
        /// <summary>
        /// Must be 2 or greater.
        /// </summary>
        public uint RangeCount { get; set; }


        [PiffArraySize(nameof(RangeCount))]
        public PiffSubsegmentIndexRange[] Ranges { get; set; }
    }
}
