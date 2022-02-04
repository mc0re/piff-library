namespace PiffLibrary.Boxes
{
    public sealed class PiffDescriptorContainer
    {
        #region Properties

        /// <summary>
        /// Determines the type of the descriptor, see <see cref="PiffEsdsBlockIds"/>.
        /// </summary>
        public byte Tag { get; set; }


        /// <summary>
        /// Length of the descriptor's data (after the length field) and all its children.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int Length { get; set; }


        [PiffDataFormat(nameof(FoundEsdTag))]
        public PiffElementaryStreamDescriptor Esd { get; set; }


        [PiffDataFormat(nameof(FoundDcdTag))]
        public PiffDecoderConfigDescription Dcd { get; set; }


        [PiffDataFormat(nameof(FoundDsiTag))]
        public PiffDecoderSpecificInfoDescriptor Dsi { get; set; }


        [PiffDataFormat(nameof(FoundSlcTag))]
        public PiffSyncLayerConfigDescription Slc { get; set; }


        /// <summary>
        /// Sub-descriptors.
        /// </summary>
        public PiffDescriptorContainer[] Children { get; set; }

        #endregion


        #region Format

        private PiffDataFormats FoundEsdTag() =>
            Tag == PiffEsdsBlockIds.Esd ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats FoundDcdTag() =>
            Tag == PiffEsdsBlockIds.Dcd ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats FoundDsiTag() =>
            Tag == PiffEsdsBlockIds.Dsi ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats FoundSlcTag() =>
            Tag == PiffEsdsBlockIds.Slc ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}