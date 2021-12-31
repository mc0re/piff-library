using System;

namespace PiffLibrary
{
    [BoxName("trak")]
    internal class PiffTrack : PiffBoxBase
    {
        #region Properties

        public PiffTrackHeader Header { get; set; }

        public PIffTrackMediaInfo Media { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for parsing.
        /// </summary>
        public PiffTrack()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffTrack(PiffTrackHeader header, PIffTrackMediaInfo info)
        {
            Header = header;
            Media = info;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrack CreateAudio(
            int trackId, PiffAudioManifest audio, DateTime created, int timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateAudio(trackId, created, audio.Duration),
                                 PIffTrackMediaInfo.CreateAudio(audio, created, audio.Duration, timeScale, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrack CreateVideo(
            int trackId, PiffVideoManifest video, DateTime created, int timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateVideo(trackId, created, video),
                                 PIffTrackMediaInfo.CreateVideo(video, created, timeScale, keyId));
        }

        #endregion
    }
}