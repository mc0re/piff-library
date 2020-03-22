using System;

namespace PiffLibrary
{
    [BoxName("trak")]
    internal class PiffTrack
    {
        #region Properties

        public PiffTrackHeader Header { get; }

        public PIffTrackMediaInfo Media { get; }

        #endregion


        #region Init and clean-up

        private PiffTrack(PiffTrackHeader header, PIffTrackMediaInfo info)
        {
            Header = header;
            Media = info;
        }


        public static PiffTrack CreateAudio(
            int trackId, PiffAudioManifest audio, DateTime created, int timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateAudio(trackId, created, audio.Duration),
                                 PIffTrackMediaInfo.CreateAudio(audio, created, audio.Duration, timeScale, keyId));
        }


        public static PiffTrack CreateVideo(
            int trackId, PiffVideoManifest video, DateTime created, int timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateVideo(trackId, created, video),
                                 PIffTrackMediaInfo.CreateVideo(video, created, timeScale, keyId));
        }

        #endregion
    }
}