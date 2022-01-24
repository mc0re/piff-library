using System;


namespace PiffLibrary.Boxes
{
    [BoxName("mdia")]
    [ChildType(typeof(PiffMediaHeaderBox))]
    [ChildType(typeof(PiffHandlerTypeBox))]
    [ChildType(typeof(PiffMediaInformationBox))]
    [ChildType(typeof(PiffExtendedLanguageBox))]
    public sealed class PIffTrackMediaInfoBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PIffTrackMediaInfoBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PIffTrackMediaInfoBox(PiffMediaHeaderBox header, PiffHandlerTypeBox handler, PiffMediaInformationBox info)
        {
            Children = new PiffBoxBase[] { header, handler, info };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PIffTrackMediaInfoBox CreateAudio(
            PiffAudioManifest audio, DateTime created, ulong duration, uint timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfoBox(
                new PiffMediaHeaderBox(created, duration, timeScale),
                new PiffHandlerTypeBox(PiffTrackTypes.Audio),
                PiffMediaInformationBox.CreateAudio(audio, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PIffTrackMediaInfoBox CreateVideo(
            PiffVideoManifest video, DateTime created, uint timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfoBox(new PiffMediaHeaderBox(created, video.Duration, timeScale),
                                          new PiffHandlerTypeBox(PiffTrackTypes.Video),
                                          PiffMediaInformationBox.CreateVideo(video, keyId));
        }

        #endregion
    }
}