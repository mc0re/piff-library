namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Marking of sync samples within the stream.
    /// If not present, every sample is a sync sample.
    /// </summary>
    [BoxName("stss")]
    public sealed class PiffSyncSampleBox : PiffFullBoxBase
    {
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public uint[] SampleNumber { get; set; }
    }
}