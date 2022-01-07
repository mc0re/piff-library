namespace PiffLibrary
{
    [BoxName("subs")]
    internal class PiffSubSampleInformation : PiffFullBoxBase
    {
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSubSampleChunk[] Samples { get; set; }
    }
}