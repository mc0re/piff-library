namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Handler-type must be "hint".
    /// </summary>
    [BoxName("fdp ")]
    public sealed class PiffFdHintSampleEntryBox : PiffSampleEntryBoxBase
    {
        public ushort HIntTrackVersion { get; set; } = 1;

        public ushort HighestCompatibleVersion { get; set; } = 1;


        /// <summary>
        /// Partition entry in <see cref="PiffPartitionEntryBox"/>.
        /// 0 means no entry is associated with this sample, e.g. for FDT.
        /// </summary>
        public ushort PartitionEntryId { get; set; }


        /// <summary>
        /// Fixed-point 8.8 value - the percentage protection overhead.
        /// </summary>
        public ushort FecOverhead { get; set; }
    }
}
