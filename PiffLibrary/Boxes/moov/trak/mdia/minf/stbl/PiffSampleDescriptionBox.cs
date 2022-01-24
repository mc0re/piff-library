using System;


namespace PiffLibrary.Boxes
{
    [BoxName("stsd")]
    [ChildType(typeof(PiffAudioSampleEntryBox))]
    [ChildType(typeof(PiffVideoSampleEntryBox))]
    [ChildType(typeof(PiffFdHintSampleEntryBox))]
    public sealed class PiffSampleDescriptionBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of <see cref="PiffSampleEntryBoxBase"/> children.
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
        private PiffSampleDescriptionBox(PiffAudioSampleEntryBox audio, PiffVideoSampleEntryBox video)
        {
            Count = 1;
            Children = new PiffBoxBase[]
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
                new PiffAudioSampleEntryBox(audio, keyId),
                null);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleDescriptionBox CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffSampleDescriptionBox(
                null,
                new PiffVideoSampleEntryBox(video, keyId));
        }

        #endregion
    }
}