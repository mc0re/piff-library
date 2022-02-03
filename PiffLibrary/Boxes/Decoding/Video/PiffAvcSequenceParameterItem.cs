namespace PiffLibrary.Boxes
{
    public sealed class PiffAvcSequenceParameterItem
    {
        /// <summary>
        /// Length of <see cref="RawData"/>.
        /// </summary>
        public short Length { get; set; }


        /// <summary>
        /// Codec sequence data.
        /// </summary>
        /// <remarks>
        /// How to interpret.
        /// 
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
        /// </remarks>
        [PiffArraySize(nameof(Length))]
        public byte[] RawData { get; set; }
    }
}
