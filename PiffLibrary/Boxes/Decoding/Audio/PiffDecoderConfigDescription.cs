namespace PiffLibrary.Boxes
{
    public sealed class PiffDecoderConfigDescription
    {
        #region Properties

        /// <summary>
        /// Type of audio.
        /// </summary>
        public byte ObjectType { get; set; } = PiffAudioObjectTypes.Mpeg4Aac;


        /// <summary>
        /// 6 higher bits - stream type:
        /// - 5 (00101) - MPEG-4 audio stream
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt6)]
        public byte StreamType { get; set; } = 5;


        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte Upstream { get; set; }


        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte Reserved { get; } = 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int BufferSizeDb { get; set; }


        public uint MaxBitRate { get; set; }


        public uint AverageBitRate { get; set; }

        #endregion
    }
}
