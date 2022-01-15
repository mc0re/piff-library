using System;
using System.Linq;


namespace PiffLibrary.Boxes
{
    [BoxName("avcC")]
    public sealed class PiffAvcConfigurationBox : PiffBoxBase
    {
        #region Constants

        private const byte NalUnitSize = 4;

        private const byte NalSequenceHeader = 0x67;

        private const byte NalPictureHeader = 0x68;

        #endregion


        #region Properties

        /// <summary>
        /// Must be 1.
        /// </summary>
        public byte ConfigurationVersion { get; set; }

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
        public byte AvcProfile { get; set; }

        public byte ProfileCompatibility { get; set; }

        public byte AvcLevel { get; set; }


        [PiffDataFormat(PiffDataFormats.Int2Minus1)]
        public byte UnitSize { get; set; }


        #region Sequence properties

        [PiffDataFormat(PiffDataFormats.Int5)]
        public byte SequenceSlotCount { get; set; }


        /// <summary>
        /// Length of <see cref="SequenceSlotData"/>
        /// </summary>
        public short SequenceSlotLength { get; set; }


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
        [PiffArraySize(nameof(SequenceSlotLength))]
        public byte[] SequenceSlotData { get; set; }

        #endregion


        #region Picture properties

        public byte PictureSlotCount { get; set; }

        public short PictureSlotLength { get; set; }

        /// <summary>
        /// Codec picture data.
        /// </summary>
        /// <remarks>
        /// How to interpret.
        /// 
        /// Remove emulation codes (03 in sequences 00 00 03 00, 00 00 03 01, 00 00 03 02, 00 00 03 03).
        ///
        /// The rest:
        ///   byte - NAL header = 0x68
        ///   golomb - PpsId (0)
        ///   golomb - SpsId (0)
        ///   bit - EntropyCoding (1)
        ///   bit - IsPictureOrderPresent (0)
        ///   golomb + 1 - SliceGroupCount (1)
        ///   golomb - NumRefIdxLevel0DefaultActiveMinus1 (2)
        ///   golomb - NumRefIdxLevel1DefaultActiveMinus1 (0)
        ///   bit - IsPredWeighted (1)
        ///   int2 - WeightedBiPred (2)
        ///   sgolomb + 26 - InitQp (0)
        ///   sgolomb + 26 - InitQs (0)
        ///   sgolomb - ChromaQpIndexOffset (0)
        ///   bit - IsDeblockingFilterPresent (1)
        ///   bit - ConstraintIntraPredicate (0)
        ///   bit - IsRedundantPictureCounterPresent (0)
        ///   
        /// Signed Golomb:
        ///   Read golomb A
        ///   If rigth bit of A is 1, return -(A >> 1)
        ///   Else return (A + 1) >> 1
        /// </remarks>
        [PiffArraySize(nameof(PictureSlotLength))]
        public byte[] PictureSlotData { get; set; }

        #endregion

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffAvcConfigurationBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffAvcConfigurationBox(string codecId, byte[] codecData)
        {
            if (codecId != "H264")
                throw new ArgumentException($"Cannot process codec '{codecId}', only 'H264' is supported.");

            var one = codecData.GetInt32(0);
            if (one != 1)
                throw new ArgumentException($"I don't know how to interpret number {one} at offset 0.");

            var seqId = codecData[4];
            if (seqId != NalSequenceHeader)
                throw new ArgumentException($"I don't know how to interpret header 0x{seqId:X} at offset 4.");

            ConfigurationVersion = 1;
            AvcProfile = codecData[5];
            ProfileCompatibility = codecData[6];
            AvcLevel = codecData[7];
            UnitSize = NalUnitSize;
            SequenceSlotCount = 1;

            var seqEnd = 8;
            while (codecData.GetInt32(seqEnd) != 1)
                seqEnd++;

            SequenceSlotData = codecData.Skip(4).Take(seqEnd - 4).ToArray();
            SequenceSlotLength = (short) SequenceSlotData.Length;

            var picId = codecData[seqEnd + 4];
            if (picId != NalPictureHeader)
                throw new ArgumentException($"I don't know how to interpret header 0x{picId:X} at offset {seqEnd + 4}.");

            PictureSlotCount = 1;
            PictureSlotData = codecData.Skip(seqEnd + 4).ToArray();
            PictureSlotLength = (short) PictureSlotData.Length;
        }

        #endregion
    };
}
