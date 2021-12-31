using System;

namespace PiffLibrary
{
    [BoxName("stbl")]
    internal class PiffSampleTable : PiffBoxBase
    {
        #region Properties

        public PiffDecodingTimeToSample Decoding { get; } = new PiffDecodingTimeToSample();

        public PiffCompositionTimeToSample Composition { get; } = new PiffCompositionTimeToSample();

        public PiffSampleToChunk Samples { get; } = new PiffSampleToChunk();

        public PiffChunkOffset Chunks { get; } = new PiffChunkOffset();

        public PiffSampleSize Sizes { get; } = new PiffSampleSize();

        public PiffSampleDescription Description { get; }

        #endregion


        #region Init and clean-up

        private PiffSampleTable(PiffSampleDescription description)
        {
            Description = description;
        }


        public static PiffSampleTable CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffSampleTable(PiffSampleDescription.CreateAudio(audio, keyId));
        }


        public static PiffSampleTable CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffSampleTable(
                PiffSampleDescription.CreateVideo(video, keyId));
        }

        #endregion
    }
}