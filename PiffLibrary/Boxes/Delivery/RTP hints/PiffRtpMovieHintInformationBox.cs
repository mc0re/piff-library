namespace PiffLibrary.Boxes
{
    [BoxName("rtp ")]
    [ChildType(typeof(PiffHintInformationBox))]
    public sealed class PiffRtpMovieHintInformationBox : PiffBoxBase
    {
        [PiffStringLength(4)]
        public string DescriptionFormat { get; set; } = "sdp ";


        /// <summary>
        /// Multiline text as defined by SDP.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string Description { get; set; }
    }
}
