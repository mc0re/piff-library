using System;

namespace PiffLibrary
{
    [BoxName("stbl")]
    [ChildType(typeof(PiffSampleDescription))]
    [ChildType(typeof(PiffDegradationPriority))]
    [ChildType(typeof(PiffDecodingTimeToSample))]
    [ChildType(typeof(PiffCompositionTimeToSample))]
    [ChildType(typeof(PiffCompositionToDecode))]
    [ChildType(typeof(PiffSyncSample))]
    [ChildType(typeof(PiffShadowSyncSample))]
    [ChildType(typeof(PiffSampleDependency))]
    [ChildType(typeof(PiffSampleSize))]
    [ChildType(typeof(PiffCompactSampleSize))]
    [ChildType(typeof(PiffSampleToChunk))]
    [ChildType(typeof(PiffChunkOffset))]
    [ChildType(typeof(PiffChunkOffset64))]
    [ChildType(typeof(PiffPaddingBits))]
    [ChildType(typeof(PiffSubSampleInformation))]
    internal class PiffSampleTable : PiffBoxBase
    {
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
            Childen = new PiffBoxBase[]
            {
                new PiffDecodingTimeToSample(),
                new PiffCompositionTimeToSample(),
                new PiffSampleToChunk(),
                new PiffChunkOffset(),
                new PiffSampleSize(),
                description
            };
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