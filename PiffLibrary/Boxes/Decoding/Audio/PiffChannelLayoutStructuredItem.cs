namespace PiffLibrary.Boxes
{
    public sealed class PiffChannelLayoutStructuredItem
    {
        #region Properties

        /// <summary>
        /// ChannelConfiguration from ISO/IEC 23001‐8.
        /// </summary>
        public byte DefinedLayout { get; set; }


        [PiffDataFormat(nameof(UseChannels))]
        public PiffChannelLayoutChannelItem[] Channels { get; set; }


        /// <summary>
        /// Bit-map of omitted channels.
        /// BIts mean: 0 - present, 1 - "not in this track".
        /// </summary>
        [PiffDataFormat(nameof(UseObject))]
        public ulong ChannelsMap { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats UseChannels() =>
            DefinedLayout == 0 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats UseObject() =>
            DefinedLayout != 0 ? PiffDataFormats.UInt64 : PiffDataFormats.Skip;

        #endregion
    }
}