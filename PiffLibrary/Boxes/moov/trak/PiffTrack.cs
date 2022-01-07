using System;

namespace PiffLibrary
{
    [BoxName("trak")]
    [ChildType(typeof(PiffTrackHeader))]
    [ChildType(typeof(PiffTrackReference))]
    [ChildType(typeof(PiffTrackGroup))]
    [ChildType(typeof(PIffTrackMediaInfo))]
    [ChildType(typeof(PiffEditList))]
    internal class PiffTrack : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffTrack()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffTrack(PiffTrackHeader header, PIffTrackMediaInfo info)
        {
            Childen = new PiffBoxBase[] { header, info };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrack CreateAudio(
            uint trackId, PiffAudioManifest audio, DateTime created, uint timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateAudio(trackId, created, audio.Duration),
                                 PIffTrackMediaInfo.CreateAudio(audio, created, audio.Duration, timeScale, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrack CreateVideo(
            uint trackId, PiffVideoManifest video, DateTime created, uint timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateVideo(trackId, created, video),
                                 PIffTrackMediaInfo.CreateVideo(video, created, timeScale, keyId));
        }

        #endregion
    }
}