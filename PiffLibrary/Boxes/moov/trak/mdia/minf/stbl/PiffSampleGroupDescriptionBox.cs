namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Characteristics of a single sample grouping.
    /// </summary>
    [BoxName("sgpd")]
    public sealed class PiffSampleGroupDescriptionBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Defines the grouping type and the corresponding entries type.
        /// </summary>
        public uint GroupingType { get; set; }


        /// <summary>
        /// Length of every group entry, if it is constant.
        /// 0 if variable (not possible with version 0).
        /// </summary>
        [PiffDataFormat(nameof(UseDefaultLength))]
        public uint DefaultLength { get; set; }


        /// <summary>
        /// Index of the entry in <see cref="Entries"/>, which applies
        /// to all samples without group (see <see cref="PiffSampleToGroupBox"/>.
        /// 0 - samples are not mapped to any group.
        /// </summary>
        [PiffDataFormat(nameof(UseDefaultIndex))]
        public uint DefaultIndex { get; set; }


        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSampleGroupDescriptionItem[] Entries { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats UseDefaultLength() =>
            Version == 1 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats UseDefaultIndex() =>
            Version >= 2 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}
