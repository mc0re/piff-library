namespace PiffLibrary
{
    internal class PiffSubSampleChunk
    {
        #region Properties

        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffSubSampleInformation Parent { get; set; }


        /// <summary>
        /// The sample number of the sample having subsample structure.
        /// Difference from the previous entry or 0 for the first entry.
        /// </summary>
        public uint Delta { get; set; }


        /// <summary>
        /// The number of subsamples in the current sample.
        /// </summary>
        public ushort Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSubSampleItem[] SubSamples { get; set; }

        #endregion


        #region Init and clean-up

        public PiffSubSampleChunk(PiffSubSampleInformation parent)
        {
            Parent = parent;
        }

        #endregion
    }
}