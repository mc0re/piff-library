using System;

namespace PiffLibrary
{
    [BoxName("stbl")]
    internal class PiffSampleTable
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


        public static PiffSampleTable CreateAudio(
            short channels, short bitsPerSample, ushort samplingRate,
            short streamId, int bitRate, byte[] codecData, Guid keyId)
        {
            return new PiffSampleTable(
                PiffSampleDescription.CreateAudio(channels, bitsPerSample, samplingRate,
                                                  streamId, bitRate, codecData, keyId));
        }


        public static PiffSampleTable CreateVideo(short width, short height, Guid keyId)
        {
            return new PiffSampleTable(
                PiffSampleDescription.CreateVideo(width, height, keyId));
        }

        #endregion
    }
}