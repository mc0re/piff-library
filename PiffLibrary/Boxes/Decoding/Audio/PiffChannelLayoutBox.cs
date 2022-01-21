namespace PiffLibrary.Boxes
{
    [BoxName("chnl")]
    public sealed class PiffChannelLayoutBox : PiffFullBoxBase
    {
        #region Properties

        public byte StreamStructure { get; set; }

        
        [PiffDataFormat(nameof(UseChannel))]
        public PiffChannelLayoutStructuredItem Layout { get; set; }

        
        [PiffDataFormat(nameof(UseObject))]
        public byte Count { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats UseChannel() =>
            (StreamStructure & 1) != 0 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats UseObject() =>
            (StreamStructure & 2) != 0 ? PiffDataFormats.UInt8 : PiffDataFormats.Skip;

        #endregion
    }
}