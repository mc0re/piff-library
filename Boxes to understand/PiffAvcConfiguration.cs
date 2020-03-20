namespace PiffLibrary
{
    [BoxName("avcC")]
    internal class PiffAvcConfiguration
    {
        public byte ConfigurationVersion { get; } = 1;

        /// <summary>
        /// Profiles:
        ///  0x42: Baseline
	    ///  0x4D: Main
	    ///  0x53: Scalable Baseline
	    ///  0x56: Scalable High
	    ///  0x58: Extended
	    ///  0x64: High
	    ///  0x6E: High 10
	    ///  0x7A: High 4:2:2
	    ///  0x90, 0xF4: High 4:4:4
        /// </summary>
        public byte AvcProfile { get; } = 0x64;

        public byte ProfileCompatibility { get; } = 0x40;

        public byte AvcLevel { get; } = 13;

        /// <summary>
        /// The left-most 6 bits are ignored (set to 1 when writing).
        /// The actual size is the right-most 2 bits + 1, here 4.
        /// </summary>
        public byte NalUnitSize { get; } = 0xFF;

        /// <summary>
        /// The left-most 3 bits are ignored (set to 1 when writing).
        /// </summary>
        public byte SequenceSlotCount { get; } = 0xE1;

        public short SequenceSlotLength { get; } = 53;

        /// <summary>
        /// Remove emulation codes (03 in sequences 00 00 03 00, 00 00 03 01, 00 00 03 02, 00 00 03 03).
        /// The rest:
        ///   byte - NAL header = 0x67
        ///   byte - AvcProfile
        ///   byte - ProfileCompatibility
        ///   byte - AvcLevel
        ///   ue - SubsetSpsId
        ///         find first non-zero 8-bit sequence R, skipping Z zero bytes
        ///         R defines the number B as the number leading zero bits in R
        ///         start reading from R, skip B bits
        ///         read (Z * 8 + B + 1) bits
        ///         subtract 1
        ///   
        /// </summary>
        public byte[] SequenceSlotData { get; } = new byte[]
        {
            0x67, 0x64, 0x00, 0x0D, 0xAC, 0x2C, 0xA5, 0x0E,
            0x11, 0xBF, 0xF0, 0x40, 0x00, 0x3F, 0x05, 0x20,
            0xC0, 0xC0, 0xC8, 0x00, 0x00, 0x1F, 0x48, 0x00,
            0x05, 0xDC, 0x03, 0x00, 0x00, 0x1C, 0x12, 0x00,
            0x03, 0x82, 0x73, 0xF8, 0xC7, 0x18, 0x00, 0x00,
            0xE0, 0x90, 0x00, 0x1C, 0x13, 0x9F, 0xC6, 0x38,
            0x76, 0x84, 0x89, 0x45, 0x80
        };


        public byte PictureSlotCount { get; } = 1;

        public short PictureSlotLength { get; } = 5;

        public byte[] PictureSlotData { get; } = new byte[]
        {
            0x68, 0xE9, 0x09, 0x35, 0x25
        };
    };
}
