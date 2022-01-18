namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Declares media type of the track.
    /// </summary>
    [BoxName("hdlr")]
    public sealed class PiffHandlerTypeBox : PiffFullBoxBase
    {
        #region Properties

        public int Reserved1 { get; }


        /// <summary>
        /// Determines, what does the parent box contain,
        /// and which sample types to expect in <see cref="PiffSampleDescriptionBox"/>.
        /// - "soun" - audio
        /// - "vide" - video
        /// - "auxv" - auxiliary video
        /// - "null" - resources
        /// 
        /// - "mp7t" - textual metadata in Unicode
        /// - "mp7b" - binary [XML] metadata in BIM format
        /// </summary>
        [PiffStringLength(4)]
        public string HandlerType { get; set; }


        [PiffArraySize(3)]
        public int[] Reserved2 { get; } = { 0, 0, 0 };


        /// <summary>
        /// Human-readable name.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string DisplayName { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffHandlerTypeBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffHandlerTypeBox(PiffTrackTypes trackType)
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