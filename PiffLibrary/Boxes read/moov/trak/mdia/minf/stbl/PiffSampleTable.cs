using System;

namespace PiffLibrary
{
    [BoxName("stbl")]
    internal class PiffSampleTable : PiffBoxBase
    {
        #region Properties

        public PiffDecodingTimeToSample Decoding { get; set; }

        public PiffCompositionTimeToSample Composition { get; set; }

        public PiffSampleToChunk Samples { get; set; }

        public PiffChunkOffset Chunks { get; set; }

        public PiffSampleSize Sizes { get; set; }

        public PiffSampleDescription Description { get; set; }

        public PiffSampleDependency Dependencies { get; set; }

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
            Decoding = new PiffDecodingTimeToSample();
            Composition = new PiffCompositionTimeToSample();
            Samples = new PiffSampleToChunk();
            Chunks = new PiffChunkOffset();
            Sizes = new PiffSampleSize();
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