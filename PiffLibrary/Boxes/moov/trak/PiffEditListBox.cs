namespace PiffLibrary.Boxes
{
    /// <summary>
    /// An explicit timeline map.
    /// </summary>
    [BoxName("elst")]
    public sealed class PiffEditListBox : PiffFullBoxBase
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
        public PiffEditListEntry[] Entries { get; set; }

        #endregion
    }
}
