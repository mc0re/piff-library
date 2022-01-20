namespace PiffLibrary.Boxes
{
    [BoxName("sdp ")]
    [ChildType(typeof(PiffHintInformationBox))]
    public sealed class PiffRtpTrackSdpHintInformationBox : PiffBoxBase
    {
        /// <summary>
        /// Multiline text as defined by SDP.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string Description { get; set; }
    }
}
