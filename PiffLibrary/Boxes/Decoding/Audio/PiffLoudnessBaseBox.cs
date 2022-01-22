namespace PiffLibrary.Boxes
{
    public abstract class PiffLoudnessBaseBox : PiffFullBoxBase
    {
        [PiffDataFormat(PiffDataFormats.UInt3)]
        public byte Reserved { get; }

        /// <summary>
        /// Matches <see cref="PiffDownmixInstructionsBox.DownmixId"/>.
        /// Or 0 for layout without downmix.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt7)]
        public byte DownmixId { get; set; }

        /// <summary>
        /// Matches a DRC box.
        /// Or 0 when not applying DRC.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt6)]
        public byte DRCSetId { get; set; }


        /// <summary>
        /// Sample peak level as defined in ISO/IEC 23003-4.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int12)]
        public short BsSamplePeakLevel { get; set; }


        /// <summary>
        /// True peak level as defined in ISO/IEC 23003-4.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int12)]
        public short BsTruePeakLevel { get; set; }


        /// <summary>
        /// Index for the measurement system as defined in ISO/IEC 23003-4.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte MeasurementSystemForTP { get; set; }


        /// <summary>
        /// One of:
        /// - 0 - unknown
        /// - 1 - unverified
        /// - 2 - "not to exceed" ceiling
        /// - 3 - measured and accurate
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte ReliabilityForTP { get; set; }
        

        public byte MeasurementCount { get; set; }


        [PiffArraySize(nameof(MeasurementCount))]
        public PiffLoudnessMeasurementItem[] Measurements { get; set; }
    }
}
