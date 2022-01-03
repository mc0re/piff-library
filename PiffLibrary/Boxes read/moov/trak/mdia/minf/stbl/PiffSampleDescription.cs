using System;

namespace PiffLibrary
{
    [BoxName("stsd")]
    internal class PiffSampleDescription : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        /// <summary>
        /// The number of elements in the following array
        /// (implemented as two mutually exclusive properties).
        /// </summary>
        public int Count { get; set; } = 1;


        public PiffProtectedAudioSampleEntry AudioEntry { get; set; }


        public PiffProtectedVideoSampleEntry VideoEntry { get; set; }

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
        /// </summary>
        private PiffSampleDescription(PiffProtectedAudioSampleEntry audio, PiffProtectedVideoSampleEntry video)
        {
            AudioEntry = audio;
            VideoEntry = video;
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