using System;

namespace PiffLibrary
{
    [BoxName("mdia")]
    internal class PIffTrackMediaInfo : PiffBoxBase
    {
        #region Properties

        public PiffMediaHeader Header { get; }

        public PiffHandlerType Handler { get; }

        public PiffMediaInformation Info { get; }

        #endregion


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
            Header = header;
            Handler = handler;
            Info = info;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PIffTrackMediaInfo CreateAudio(
            PiffAudioManifest audio, DateTime created, long duration, int timeScale, Guid keyId)
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
            PiffVideoManifest video, DateTime created, int timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfo(new PiffMediaHeader(created, video.Duration, timeScale),
                                          new PiffHandlerType(PiffTrackTypes.Video),
                                          PiffMediaInformation.CreateVideo(video, keyId));
        }

        #endregion
    }
}