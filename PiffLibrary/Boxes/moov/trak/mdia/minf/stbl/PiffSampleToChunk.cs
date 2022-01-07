namespace PiffLibrary
{
    /// <summary>
    /// Samples are grouped into chunks.
    /// </summary>
    [BoxName("stsc")]
    internal class PiffSampleToChunk : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of elements in the following array.
        /// </summary>
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSampleToChunkItem[] Items { get; set; }

        #endregion
    }

    internal class PiffSampleToChunkItem
    {
        /// <summary>
        /// Index of the first chunk of a run of chunks with the same
        /// <see cref="SamplesPerChunk"/> and <see cref="SampleDescription"/>
        /// characteristics.
        /// 1-based.
        /// </summary>
        public uint FirstChunk { get; set; }


        /// <summary>
        /// The number of samples in each chunk.
        /// </summary>
        public uint SamplesPerChunk { get; set; }


        /// <summary>
        /// Index of the sample entry that describes the samples in each chunk.
        /// See <see cref="PiffSampleDescription"/>.
        /// </summary>
        public uint SampleDescription { get; set; }
    }
}