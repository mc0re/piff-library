namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Associate file group names to file group IDs.
    /// </summary>
    [BoxName("gitn")]
    public sealed class PiffGroupIdToNameBox : PiffFullBoxBase
    {
        public ushort Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffGroupIdToNameItem[] Items { get; set; }
    }
}
