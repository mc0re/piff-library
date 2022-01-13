using System;


namespace PiffLibrary.Boxes
{
    [BoxName("stsd")]
    [ChildType(typeof(PiffProtectedAudioSampleEntryBox))]
    [ChildType(typeof(PiffProtectedVideoSampleEntryBox))]
    public sealed class PiffSampleDescriptionBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of sample tracks.
        /// </summary>
        public uint Count { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffSampleDescriptionBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// Only one of the properties must be present.
        /// </summary>
        private PiffSampleDescriptionBox(PiffProtectedAudioSampleEntryBox audio, PiffProtectedVideoSampleEntryBox video)
        {
            Count = 1;
            Childen = new PiffBoxBase[]
            {
                audio is null ? (PiffBoxBase)video : audio
            };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleDescriptionBox CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffSampleDescriptionBox(
                new PiffProtectedAudioSampleEntryBox(audio, keyId),
                null);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleDescriptionBox CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffSampleDescriptionBox(
                null,
                new PiffProtectedVideoSampleEntryBox(video, keyId));
        }

        #endregion
    }
}