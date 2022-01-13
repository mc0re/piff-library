namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Samples are grouped into chunks.
    /// </summary>
    [BoxName("stsc")]
    public sealed class PiffSampleToChunkBox : PiffFullBoxBase
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
}