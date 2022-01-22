namespace PiffLibrary.Boxes
{
    public sealed class PiffLoudnessMeasurementItem
    {
        /// <summary>
        /// Index for the measurement method as defined in ISO/IEC 23003-4.
        /// </summary>
        public byte MethodDefinition { get; set; }


        public byte MethodValue { get; set; }


        /// <summary>
        /// Index for the measurement system as defined in ISO/IEC 23003-4.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte MeasurementSystem { get; set; }


        /// <summary>
        /// One of:
        /// - 0 - unknown
        /// - 1 - unverified
        /// - 2 - "not to exceed" ceiling
        /// - 3 - measured and accurate
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt4)]
        public byte Reliability { get; set; }
    }
}
