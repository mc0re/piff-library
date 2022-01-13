using System;


namespace PiffLibrary.Boxes
{
    [BoxName("trak")]
    [ChildType(typeof(PiffTrackHeaderBox))]
    [ChildType(typeof(PiffTrackReferenceBox))]
    [ChildType(typeof(PiffTrackGroupBox))]
    [ChildType(typeof(PIffTrackMediaInfoBox))]
    [ChildType(typeof(PiffEditListBox))]
    [ChildType(typeof(PiffUserDataBox))]
    [ChildType(typeof(PiffMetadataBox))]
    public sealed class PiffTrackBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffTrackBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffTrackBox(PiffTrackHeaderBox header, PIffTrackMediaInfoBox info)
        {
            Childen = new PiffBoxBase[] { header, info };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrackBox CreateAudio(
            uint trackId, PiffAudioManifest audio, DateTime created, uint timeScale, Guid keyId)
        {
            return new PiffTrackBox(PiffTrackHeaderBox.CreateAudio(trackId, created, audio.Duration),
                                 PIffTrackMediaInfoBox.CreateAudio(audio, created, audio.Duration, timeScale, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffTrackBox CreateVideo(
            uint trackId, PiffVideoManifest video, DateTime created, uint timeScale, Guid keyId)
        {
            return new PiffTrackBox(PiffTrackHeaderBox.CreateVideo(trackId, created, video),
                                 PIffTrackMediaInfoBox.CreateVideo(video, created, timeScale, keyId));
        }

        #endregion
    }
}