using System;


namespace PiffLibrary.Boxes
{
    [BoxName("encv")]
    [ChildType(typeof(PiffProtectionSchemeInformationBox))]
    [ChildType(typeof(PiffAvcConfigurationBox))]
    [ChildType(typeof(PiffPixelAspectRatioBox))]
    [ChildType(typeof(PiffCleanApertureBox))]
    [ChildType(typeof(PiffColourInformationBox))]
    public sealed class PiffVideoSampleEntryBox : PiffSampleEntryBoxBase
    {
        #region Properties

        [PiffArraySize(2)]
        public short[] Reserved2 { get; } = { 0, 0 };


        [PiffArraySize(3)]
        public int[] Reserved3 { get; } = { 0, 0, 0 };


        /// <summary>
        /// Maximum width of the stream in pixels.
        /// </summary>
        public ushort Width { get; set; }


        /// <summary>
        /// Maximum height of the stream in pixels.
        /// </summary>
        public ushort Height { get; set; }


        /// <summary>
        /// Fixed 16.16 [dpi].
        /// 0x48.0000 = 72 dpi.
        /// </summary>
        public uint HorizontalResolution { get; set; } = 0x480000;


        /// <summary>
        /// Fixed 16.16 [dpi].
        /// 0x48.0000 = 72 dpi.
        /// </summary>
        public uint VerticalResolution { get; set; } = 0x480000;


        public int Reserved4 { get; }


        /// <summary>
        /// Frames per sample.
        /// </summary>
        public ushort FrameCount { get; set; } = 1;


        /// <summary>
        /// Fixed-size field for display purposes.
        /// First byte is string length. Padded with 0.
        /// </summary>
        [PiffArraySize(32)]
        public byte[] CompressorName { get; set; }


        /// <summary>
        /// 0x18 = colour, no alpha
        /// </summary>
        public ushort Depth { get; set; } = 0x18;


        public short Reserved5 { get; } = -1;

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffVideoSampleEntryBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffVideoSampleEntryBox(PiffVideoManifest video, Guid keyId)
        {
            DataReferenceIndex = 1;
            Width = video.Width;
            Height = video.Height;
            CompressorName = new byte[32];
            var scheme = PiffProtectionSchemeInformationBox.CreateVideo(video.CodecId, keyId);
            var config = new PiffAvcConfigurationBox(video.CodecId, video.CodecData);
            Children = new PiffBoxBase[] { config, scheme };
        }

        #endregion
    }
}