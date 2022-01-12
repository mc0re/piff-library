using PiffLibrary.Boxes;
using System;

namespace PiffLibrary
{
    [BoxName("stbl")]
    [ChildType(typeof(PiffSampleDescription))]
    [ChildType(typeof(PiffDegradationPriorityBox))]
    [ChildType(typeof(PiffDecodingTimeToSampleBox))]
    [ChildType(typeof(PiffCompositionTimeToSampleBox))]
    [ChildType(typeof(PiffCompositionToDecodeBox))]
    [ChildType(typeof(PiffSyncSample))]
    [ChildType(typeof(PiffShadowSyncSample))]
    [ChildType(typeof(PiffSampleDependency))]
    [ChildType(typeof(PiffSampleSize))]
    [ChildType(typeof(PiffCompactSampleSizeBox))]
    [ChildType(typeof(PiffSampleToChunk))]
    [ChildType(typeof(PiffChunkOffsetBox))]
    [ChildType(typeof(PiffChunkOffset64Box))]
    [ChildType(typeof(PiffPaddingBits))]
    [ChildType(typeof(PiffSubSampleInformationBox))]
    [ChildType(typeof(PiffSampleAuxiliaryInformation))]
    [ChildType(typeof(PiffSampleAuxiliaryOffset))]
    [ChildType(typeof(PiffSampleToGroupBox))]
    [ChildType(typeof(PiffSampleGroupDescriptionBox))]
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
                new PiffDecodingTimeToSampleBox(),
                new PiffCompositionTimeToSampleBox(),
                new PiffSampleToChunk(),
                new PiffChunkOffsetBox(),
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