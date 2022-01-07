namespace PiffLibrary
{
    /// <summary>
    /// Declares media type of the track.
    /// </summary>
    [BoxName("hdlr")]
    internal class PiffHandlerType : PiffFullBoxBase
    {
        #region Properties

        public int Reserved1 { get; }


        /// <summary>
        /// What does the parent box contain:
        /// - "soun" - audio
        /// - "vide" - video
        /// - "null" - resources
        /// </summary>
        [PiffStringLength(4)]
        public string HandlerType { get; set; }


        [PiffArraySize(3)]
        public int[] Reserved2 { get; } = { 0, 0, 0 };


        /// <summary>
        /// 0-terminated UTF-8 human-readable name.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string DisplayName { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffHandlerType()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffHandlerType(PiffTrackTypes trackType)
        {
            if (trackType == PiffTrackTypes.Audio)
            {
                HandlerType = "soun";
                DisplayName = "Audio";
            }
            else
            {
                HandlerType = "vide";
                DisplayName = "Video";
            }
        }

        #endregion
    }
}