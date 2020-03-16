namespace PiffLibrary
{
    [BoxName("hdlr")]
    internal class PiffHandlerType
    {
        #region Properties

        public byte Version { get; } = 0;

        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;

        public int Reserved1 { get; } = 0;

        /// <summary>
        /// 4 bytes.
        /// "soun" for audio track, "vide" for video.
        /// </summary>
        public string HandlerType { get; set; }

        public int[] Reserved2 { get; } = { 0, 0, 0 };

        /// <summary>
        /// 0-terminated UTF-8 human-readable name.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string Name { get; }

        #endregion


        #region Init and clean-up

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