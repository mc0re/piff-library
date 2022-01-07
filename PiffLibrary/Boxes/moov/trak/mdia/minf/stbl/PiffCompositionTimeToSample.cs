namespace PiffLibrary
{
    [BoxName("ctts")]
    internal class PiffCompositionTimeToSample : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of elements in the following array.
        /// </summary>
        public uint Count { get; set; }


        /// <summary>
        /// Offsets between decoding time and composition time.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        public CompositionSampleItem[] Offsets { get; set; }

        #endregion
    }
}