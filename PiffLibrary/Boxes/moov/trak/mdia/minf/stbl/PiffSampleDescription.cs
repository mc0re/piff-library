using System;
using System.Drawing;

namespace PiffLibrary
{
    [BoxName("stsd")]
    [ChildType(typeof(PiffProtectedAudioSampleEntry))]
    [ChildType(typeof(PiffProtectedVideoSampleEntry))]
    internal class PiffSampleDescription : PiffFullBoxBase
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
        public PiffSampleDescription()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// Only one of the properties must be present.
        /// </summary>
        private PiffSampleDescription(PiffProtectedAudioSampleEntry audio, PiffProtectedVideoSampleEntry video)
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
        public static PiffSampleDescription CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffSampleDescription(
                new PiffProtectedAudioSampleEntry(audio, keyId),
                null);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleDescription CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffSampleDescription(
                null,
                new PiffProtectedVideoSampleEntry(video, keyId));
        }

        #endregion
    }
}