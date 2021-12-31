using System;

namespace PiffLibrary
{
    [BoxName("stbl")]
    internal class PiffSampleTable : PiffBoxBase
    {
        #region Properties

        public PiffDecodingTimeToSample Decoding { get; set; } = new PiffDecodingTimeToSample();

        public PiffCompositionTimeToSample Composition { get; set; } = new PiffCompositionTimeToSample();

        public PiffSampleToChunk Samples { get; set; } = new PiffSampleToChunk();

        public PiffChunkOffset Chunks { get; set; } = new PiffChunkOffset();

        public PiffSampleSize Sizes { get; set; } = new PiffSampleSize();

        public PiffSampleDescription Description { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffSampleTable()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffSampleTable(PiffSampleDescription description)
        {
            Description = description;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleTable CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffSampleTable(PiffSampleDescription.CreateAudio(audio, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleTable CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffSampleTable(PiffSampleDescription.CreateVideo(video, keyId));
        }

        #endregion
    }
}