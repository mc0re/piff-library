namespace PiffLibrary
{
    internal class CompositionSampleItem
    {
        #region Fields

        /// <summary>
        /// Explicit parent.
        /// </summary>
        private readonly PiffCompositionTimeToSample mParent;

        #endregion


        #region Properties

        /// <summary>
        /// The number of consecutive samples this offset applies.
        /// </summary>
        public uint Count { get; set; }


        /// <summary>
        /// Offset in media time scale.
        /// </summary>
        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long Offset { get; set; }

        #endregion


        #region Init and clean-up

        public CompositionSampleItem(PiffCompositionTimeToSample parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        public PiffDataFormats GetOffsetFormat() =>
            mParent.Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Int32;

        #endregion
    }
}
