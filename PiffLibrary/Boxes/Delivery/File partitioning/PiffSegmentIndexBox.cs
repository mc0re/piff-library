namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Division of a segment into sub-segments.
    /// </summary>
    [BoxName("sidx")]
    public sealed class PiffSegmentIndexBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Stream ID for the reference stream.
        /// </summary>
        public uint ReferenceId { get; set; }


        /// <summary>
        /// Time scale, in ticks per second, for <see cref="EarliestPresentationTime"/>
        /// and <see cref="PiffSegmentIndexItem.SubsegmentDuration"/>.
        /// Recommended to match <see cref="PiffMediaHeaderBox.TimeScale"/>.
        /// </summary>
        public uint TimeScale { get; set; }


        /// <summary>
        /// Earliest presentation time of any content in the reference stream,
        /// in <see cref="TimeScale"/> units.
        /// </summary>
        [PiffDataFormat(nameof(GetTimeFormat))]
        public ulong EarliestPresentationTime { get; set; }


        /// <summary>
        /// Offset in bytes from the anchor point to the first byte
        /// of the indexed material.
        /// </summary>
        [PiffDataFormat(nameof(GetOffsetFormat))]
        public ulong FirstOffset { get; set; }


        public ushort Reserved { get; }


        public ushort ReferenceCount { get; set; }


        [PiffArraySize(nameof(ReferenceCount))]
        public PiffSegmentIndexItem[] References { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats GetTimeFormat() =>
            Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;


        private PiffDataFormats GetOffsetFormat() =>
            Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;

        #endregion
    }
}
