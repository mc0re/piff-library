namespace PiffLibrary
{
    /// <summary>
    /// Find the group a sample belongs to.
    /// </summary>
    [BoxName("sbgp")]
    internal class PiffSampleToGroup : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Same as in <see cref="PiffSampleGroupDescription"/>.
        /// </summary>
        public uint GroupingType { get; set; }


        /// <summary>
        /// Sub-type of this grouping.
        /// </summary>
        [PiffDataFormat(nameof(UseParameter))]
        public uint GroupingTypeParameter { get; set; }


        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSampleToGroupItem[] Items { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats UseParameter() =>
            Version == 1 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}
