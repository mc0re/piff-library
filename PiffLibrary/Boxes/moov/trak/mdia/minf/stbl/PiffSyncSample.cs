using PiffLibrary.Boxes;

namespace PiffLibrary
{
    /// <summary>
    /// Marking of sync samples within the stream.
    /// If not present, every sample is a sync sample.
    /// </summary>
    [BoxName("stss")]
    internal class PiffSyncSample : PiffFullBoxBase
    {
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public uint[] SampleNumber { get; set; }
    }
}