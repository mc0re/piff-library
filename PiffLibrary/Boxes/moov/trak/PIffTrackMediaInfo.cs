using PiffLibrary.Boxes;
using System;

namespace PiffLibrary
{
    [BoxName("mdia")]
    [ChildType(typeof(PiffMediaHeader))]
    [ChildType(typeof(PiffHandlerType))]
    [ChildType(typeof(PiffMediaInformation))]
    [ChildType(typeof(PiffExtendedLanguageBox))]
    internal class PIffTrackMediaInfo : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PIffTrackMediaInfo()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PIffTrackMediaInfo(PiffMediaHeader header, PiffHandlerType handler, PiffMediaInformation info)
        {
            Childen = new PiffBoxBase[] { header, handler, info };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PIffTrackMediaInfo CreateAudio(
            PiffAudioManifest audio, DateTime created, ulong duration, uint timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfo(
                new PiffMediaHeader(created, duration, timeScale),
                new PiffHandlerType(PiffTrackTypes.Audio),
                PiffMediaInformation.CreateAudio(audio, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PIffTrackMediaInfo CreateVideo(
            PiffVideoManifest video, DateTime created, uint timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfo(new PiffMediaHeader(created, video.Duration, timeScale),
                                          new PiffHandlerType(PiffTrackTypes.Video),
                                          PiffMediaInformation.CreateVideo(video, keyId));
        }

        #endregion
    }
}