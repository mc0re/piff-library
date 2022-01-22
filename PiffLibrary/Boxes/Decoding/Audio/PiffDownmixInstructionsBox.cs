namespace PiffLibrary.Boxes
{
    /// <summary>
    /// One for each downmix operation.
    /// </summary>
    [BoxName("dmix")]
    public sealed class PiffDownmixInstructionsBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// ChannelConfiguration from ISO/IEC 23001‐8.
        /// </summary>
        public byte TargetLayout { get; set; }

        
        /// <summary>
        /// Channel count of the resulting stream.
        /// </summary>
        public byte TargetChannelCount { get; set; }


        /// <summary>
        /// Whether the coefficients are inside the stream or supplied here.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte InStream { get; set; }


        /// <summary>
        /// Downmix box ID.
        /// Must be between 1 and 0x7E.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt7)]
        public byte DownmixId { get; set; }


        /// <summary>
        /// If <see cref="InStream"/> is 1, no data is given here.
        /// If it is 0, there are <see cref="TargetChannelCount"/> * <see cref="PiffAudioSampleEntryBox.ChannelCount"/>.
        /// 
        /// Each coefficient is a multiplier for the downmix.
        /// +-----+---------+----------+
        /// | Val | Coef for| Coef for |
        /// |     | non-LFE |    LFE   |
        /// +-----+---------+----------+
        /// | 0x0 |  0.0 dB |  10.0 dB |
        /// | 0x1 | -0.5 dB |   6.0 dB |
        /// | 0x2 | -1.0 dB |   4.5 dB |
        /// | 0x3 | -1.5 dB |   3.0 dB |
        /// | 0x4 | -2.0 dB |   1.5 dB |
        /// | 0x5 | -2.5 dB |   0.0 dB |
        /// | 0x6 | -3.0 dB |  -1.5 dB |
        /// | 0x7 | -3.5 dB |  -3.0 dB |
        /// | 0x8 | -4.0 dB |  -4.5 dB |
        /// | 0x9 | -4.5 dB |  -6.0 dB |
        /// | 0xA | -5.0 dB | -10.0 dB |
        /// | 0xB | -5.5 dB | -15.0 dB |
        /// | 0xC | -6.0 dB | -20.0 dB |
        /// | 0xD | -7.5 dB | -30.0 dB |
        /// | 0xE | -9.0 dB | -40.0 dB |
        /// | 0xF | -inf    | -inf     |
        /// +-----+---------+----------+
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte[] DownmixCoefficients { get; set; }

        #endregion
    }
}