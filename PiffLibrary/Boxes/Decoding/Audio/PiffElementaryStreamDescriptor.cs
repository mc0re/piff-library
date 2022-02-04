namespace PiffLibrary.Boxes
{
    public sealed class PiffElementaryStreamDescriptor
    {
        #region Properties

        /// <summary>
        /// Stream ID.
        /// </summary>
        public ushort StreamId { get; set; }


        /// <summary>
        /// Stream dependant flag; if defined, read <see cref="DependsOnEsId"/>.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte HasStreamDependency { get; set; }


        /// <summary>
        /// URL; if defined, read <see cref="Url"/>.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte HasUrl { get; set; }


        /// <summary>
        /// OCR; if defined, read <see cref="OcrEsId"/>.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte HasOcr { get; set; }


        /// <summary>
        /// Stream priority.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt5)]
        public byte Priority { get; set; }


        [PiffDataFormat(nameof(UseDependsOn))]
        public ushort DependsOnEsId { get; set; }


        [PiffDataFormat(nameof(UseUrl))]
        public string Url { get; set; }


        [PiffDataFormat(nameof(UseOcr))]
        public ushort OcrEsId { get; set; }

        #endregion


        #region Format

        private PiffDataFormats UseDependsOn() =>
            HasStreamDependency == 1 ? PiffDataFormats.UInt16 : PiffDataFormats.Skip;


        private PiffDataFormats UseUrl() =>
            HasUrl == 1 ? PiffDataFormats.AsciiPascal : PiffDataFormats.Skip;


        private PiffDataFormats UseOcr() =>
            HasOcr == 1 ? PiffDataFormats.UInt16 : PiffDataFormats.Skip;

        #endregion
    }
}
