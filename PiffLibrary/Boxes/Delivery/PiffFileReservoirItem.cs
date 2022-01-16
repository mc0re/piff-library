namespace PiffLibrary.Boxes
{
    public sealed class PiffFileReservoirItem
    {
        #region Fields

        private readonly PiffFileReservoirBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// Location of the file reservoir.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public uint ItemId { get; set; }


        /// <summary>
        /// The number of source symbols in the file reservoir.
        /// </summary>
        public uint SymbolCount { get; set; }

        #endregion


        #region Init and clean-up

        public PiffFileReservoirItem(PiffFileReservoirBox parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetIdFormat() =>
            mParent.Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
