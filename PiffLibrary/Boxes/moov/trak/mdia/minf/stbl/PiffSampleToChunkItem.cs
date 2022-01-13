namespace PiffLibrary.Boxes
{
    public sealed class PiffSampleToChunkItem
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
        /// See <see cref="PiffSampleDescriptionBox"/>.
        /// </summary>
        public uint SampleDescription { get; set; }
    }
}