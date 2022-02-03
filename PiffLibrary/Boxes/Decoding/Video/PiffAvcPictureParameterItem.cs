namespace PiffLibrary.Boxes
{
    public sealed class PiffAvcPictureParameterItem
    {
        /// <summary>
        /// Length of <see cref="RawData"/>.
        /// </summary>
        public short Length { get; set; }


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
        [PiffArraySize(nameof(Length))]
        public byte[] RawData { get; set; }
    }
}
