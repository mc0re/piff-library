using System;

namespace PiffLibrary
{
    [BoxName("mdia")]
    internal class PIffTrackMediaInfo
    {
        #region Properties

        public PiffMediaHeader Header { get; }

        public PiffHandlerType Handler { get; }

        public PiffMediaInformation Info { get; }

        #endregion


        #region Init and clean-up

        private PIffTrackMediaInfo(PiffMediaHeader header, PiffHandlerType handler, PiffMediaInformation info)
        {
            Header = header;
            Handler = handler;
            Info = info;
        }


        public static PIffTrackMediaInfo CreateAudio(
            short trackId, PiffAudioManifest audio,
            DateTime created, long duration, int timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfo(
                new PiffMediaHeader(created, duration, timeScale),
                new PiffHandlerType(PiffTrackTypes.Audio),
                PiffMediaInformation.CreateAudio(trackId, audio, keyId));
        }


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