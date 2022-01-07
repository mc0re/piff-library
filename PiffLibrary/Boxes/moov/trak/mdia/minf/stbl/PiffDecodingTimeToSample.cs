namespace PiffLibrary
{
    [BoxName("stts")]
    internal class PiffDecodingTimeToSample : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of elements in the following array.
        /// </summary>
        public uint Count { get; set; }


        /// <summary>
        /// Indexing from decoding time to sample number.
        /// Ordered by time, thus all <see cref="PiffDecodingSampleItem.Delta"/> are non-negative.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        public PiffDecodingSampleItem[] Samples { get; set; }

        #endregion
    }
}