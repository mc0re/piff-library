namespace PiffLibrary.Boxes
{
    public sealed class PiffFecReservoirItem
    {
        #region Fields

        private readonly PiffFecReservoirBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// Location of the FEC reservoir associated with a source block.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public uint ItemId { get; set; }


        /// <summary>
        /// The number of repair symbols in the FEC reservoir.
        /// </summary>
        public uint RepairSymbolCount { get; set; }

        #endregion


        #region Init and clean-up

        public PiffFecReservoirItem(PiffFecReservoirBox parent)
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
