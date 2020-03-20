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
        public byte AvcProfile { get; } = 0x4D;

        public byte ProfileCompatibility { get; } = 0x40;

        public byte AvcLevel { get; } = 0x1E;

        [PiffDataFormat(PiffDataFormats.Int2Minus1)]
        public byte NalUnitSize { get; } = 4;

        [PiffDataFormat(PiffDataFormats.Int5)]
        public byte SequenceSlotCount { get; } = 1;


        /// <summary>
        /// Length of <see cref="SequenceSlotData"/>
        /// </summary>
        public short SequenceSlotLength { get; } = 29;


        /// <summary>
        /// Remove emulation codes (03 in sequences 00 00 03 00, 00 00 03 01, 00 00 03 02, 00 00 03 03).
        ///
        /// The rest:
        ///   byte - NAL header = 0x67
        ///   byte - AvcProfile (0x4D)
        ///   byte - ProfileCompatibility (0x40)
        ///   byte - AvcLevel (0x1E)
        ///   golomb - SubsetSpsId (0)
        ///   if AvcProfile in 0x2C(?), 0x53, 0x56, 0x64, 0x6E, 0x76(?), 0x7A, 0x80(?), 0xF4
        ///     colour information
        ///   golomb + 4 - MaxFrameNumberLog2 (4)
        ///   golomb - PocType (0)
        ///   if PocType == 0
        ///     golomb + 4 - MaxPocLsbLog2 (5)
        ///   if PocType == 1
        ///     more reads
        ///   golomb MaxNofReferenceFrames (3)
        ///   bit - AreGapsInFrameNumbersAllowed (0)
        ///   golomb + 1 - MbWidth (40 = 0x28), real width = MbWidth * 16
        ///   golomb + 1 - MbHeight (23 = 0x17), real height = MbHeight * 16 * (2 - FrameMbsOnly)
        ///   bit - FrameMbsOnly (1)
        ///   if ! FrameMbsOnly
        ///     bit - MbAdaptiveFrameField (default 0)
        ///   bit - reserved / direct_8x8_inference (1)
        ///   bit - Crop
        ///   if Crop
        ///     golomb - CropLeft (0)
        ///     golomb - CropRight (0)
        ///     golomb - CropTop (0)
        ///     golomb - CropBottom (4)
        ///     real crop values are those times multipliers depending on colour settings
        ///   bit - IsVuiPresent (1)
        ///   if IsVuiPresent
        ///     bit - IsAspectRatioPresent (1)
        ///     if IsAspectRatioPresent
        ///       byte - AspectRatioId (1)
        ///       if AspectRatioId == 255
        ///         short - Numerator (default 1)
        ///         short - Denominator (default 1)
        ///     bit - IsOverscanPresent (0)
        ///     if IsOverscanPresent
        ///       bit IsOverscanAppropriate
        ///     bit - IsVideoSignalTypePresent (1)
        ///     if IsVideoSignalTypePresent
        ///       3bits - VideoFormat (5)
        ///       bit - IsVideoFullRange (0)
        ///       bit - IsColorDescriptionPresent (1)
        ///       if IsColorDescriptionPresent
        ///         byte - ColorPrimaries (1)
        ///         byte - TransferCharacteristics (1)
        ///         byte - MatrixCoefficients (1)
        ///     bit - IsChromaLocationPresent (0)
        ///     if IsChromaLocationPresent
        ///       golomb - ChromaSampleLocationTypeTop
        ///       golomb - ChromaSampleLocationTypeBottom
        ///     bit - IsTimingPresent (1)
        ///     if IsTimingPresent
        ///       int32 - NofUnitsInTick (1)
        ///       int32 - TimeScale (50 = 0x32)
        ///       bit - IsFrameRateFixed (0)
        ///     bit - IsNalHeaderPresent (0)
        ///     if IsNalHeaderPresent
        ///       ...
        ///     bit - IsVclHeaderPresent (0)
        ///     if IsVclHeaderPresent
        ///       ...
        ///     if IsNalHeaderPresent || IsVclHeaderPresent
        ///       bit - IsLowDelayHeader
        ///     bit - IsPictStructPresent (0)
        ///
        /// Golomb:
        ///   get the number of 0 bits Z before the first 1
        ///   read (Z + 1) bits
        ///   subtract 1
        /// </summary>
        public byte[] SequenceSlotData { get; } = new byte[]
        {
            0x67, 0x4D, 0x40, 0x1E, 0xE8, 0x80, 0x50, 0x17,
            0xFC, 0xB8, 0x0B, 0x50, 0x10, 0x10, 0x14, 0x00,
            0x00, 0x03, 0x00, 0x04, 0x00, 0x00, 0x03, 0x00,
            0xC8, 0x3C, 0x58, 0xB4, 0x48
        };


        public byte PictureSlotCount { get; } = 1;

        public short PictureSlotLength { get; } = 4;

        public byte[] PictureSlotData { get; } = new byte[]
        {
            0x68, 0xEB, 0xEF, 0x20
        };
    };
}
