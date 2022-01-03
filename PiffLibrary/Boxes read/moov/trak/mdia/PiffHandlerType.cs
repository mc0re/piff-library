namespace PiffLibrary
{
    [BoxName("hdlr")]
    internal class PiffHandlerType : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        public int Reserved1 { get; } = 0;


        /// <summary>
        /// "soun" for audio track, "vide" for video.
        /// </summary>
        [PiffDataLength(4)]
        public string HandlerType { get; set; }


        [PiffArraySize(3)]
        public int[] Reserved2 { get; } = { 0, 0, 0 };


        /// <summary>
        /// 0-terminated UTF-8 human-readable name.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string Name { get; set; }

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
                Name = "Audio";
            }
            else
            {
                HandlerType = "vide";
                Name = "Video";
            }
        }

        #endregion
    }
}