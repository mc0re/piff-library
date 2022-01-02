namespace PiffLibrary
{
    [BoxName("elst")]
    internal class PiffEditListItem : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit time and duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; set; }


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; }


        /// <summary>
        /// The number of entries in <see cref="Entries"/>.
        /// </summary>
        public int EntryCount { get; set; }


        /// <summary>
        /// Individual edit entries.
        /// </summary>
        [PiffArraySize(nameof(EntryCount))]
        public PiffEditListItemEntry[] Entries { get; set; }

        #endregion
    }
}
