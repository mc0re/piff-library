namespace PiffLibrary.Boxes
{
    [BoxName("subs")]
    public sealed class PiffSubSampleInformationBox : PiffFullBoxBase
    {
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSubSampleChunk[] Samples { get; set; }
    }
}