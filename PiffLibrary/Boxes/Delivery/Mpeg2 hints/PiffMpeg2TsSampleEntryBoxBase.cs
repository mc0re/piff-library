namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Accepts the following child boxes:
    /// "tPAT"
    /// "tPMT"
    /// "tOD "
    /// "tsti"
    /// "istm"
    /// </summary>
    public abstract class PiffMpeg2TsSampleEntryBoxBase : PiffSampleEntryBoxBase
    {
        public ushort HintTrackVersion { get; set; } = 1;

        public ushort HighestCompatibleVersion { get; set; } = 1;


        /// <summary>
        /// The number of bytes preceding each MPEG2-TS packet.
        /// </summary>
        public byte PrecedingBytesLength { get; set; }

        /// <summary>
        /// The number of bytes after the end of each MPEG2-TS packet.
        /// </summary>
        public byte TrailingBytesLength { get; set; }


        /// <summary>
        /// If 1, all associated samples are pre-computed.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt1)]
        public byte PrecomputedOnly { get; set; }


        [PiffDataFormat(PiffDataFormats.UInt7)]
        public byte Reserved1 { get; }
    }
}
