namespace PiffLibrary
{
    /// <summary>
    /// An explicit timeline map.
    /// </summary>
    [BoxName("elst")]
    internal class PiffEditListItem : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of entries in <see cref="Entries"/>.
        /// </summary>
        public uint EntryCount { get; set; }


        /// <summary>
        /// Individual edit entries.
        /// </summary>
        [PiffArraySize(nameof(EntryCount))]
        public PiffEditListItemEntry[] Entries { get; set; }

        #endregion
    }
}
