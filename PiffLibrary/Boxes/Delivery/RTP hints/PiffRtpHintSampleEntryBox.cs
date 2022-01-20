namespace PiffLibrary.Boxes
{
    [BoxName("rtp ")]
    [ChildType(typeof(PiffTimeScaleEntryBox))]
    [ChildType(typeof(PiffTimeOffsetBox))]
    [ChildType(typeof(PiffSequenceOffsetBox))]
    public sealed class PiffRtpHintSampleEntryBox : PiffSampleEntryBoxBase
    {
        public ushort HintTrackVersion { get; set; } = 1;


        /// <summary>
        /// The oldest version with which this track is backward-compatible.
        /// </summary>
        public ushort HighestCompatibleVersion { get; set; } = 1;


        /// <summary>
        /// The size of the largest packet this track will generate.
        /// </summary>
        public uint MaxPacketSize { get; set; }
    }
}
