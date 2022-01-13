using System;


namespace PiffLibrary.Boxes
{
    [BoxName("stbl")]
    [ChildType(typeof(PiffSampleDescriptionBox))]
    [ChildType(typeof(PiffDegradationPriorityBox))]
    [ChildType(typeof(PiffDecodingTimeToSampleBox))]
    [ChildType(typeof(PiffCompositionTimeToSampleBox))]
    [ChildType(typeof(PiffCompositionToDecodeBox))]
    [ChildType(typeof(PiffSyncSampleBox))]
    [ChildType(typeof(PiffShadowSyncSampleBox))]
    [ChildType(typeof(PiffSampleDependencyBox))]
    [ChildType(typeof(PiffSampleSizeBox))]
    [ChildType(typeof(PiffCompactSampleSizeBox))]
    [ChildType(typeof(PiffSampleToChunkBox))]
    [ChildType(typeof(PiffChunkOffsetBox))]
    [ChildType(typeof(PiffChunkOffset64Box))]
    [ChildType(typeof(PiffPaddingBitsBox))]
    [ChildType(typeof(PiffSubSampleInformationBox))]
    [ChildType(typeof(PiffSampleAuxiliaryInformationBox))]
    [ChildType(typeof(PiffSampleAuxiliaryOffsetBox))]
    [ChildType(typeof(PiffSampleToGroupBox))]
    [ChildType(typeof(PiffSampleGroupDescriptionBox))]
    public sealed class PiffSampleTableBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffSampleTableBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffSampleTableBox(PiffSampleDescriptionBox description)
        {
            Childen = new PiffBoxBase[]
            {
                new PiffDecodingTimeToSampleBox(),
                new PiffCompositionTimeToSampleBox(),
                new PiffSampleToChunkBox(),
                new PiffChunkOffsetBox(),
                new PiffSampleSizeBox(),
                description
            };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleTableBox CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffSampleTableBox(PiffSampleDescriptionBox.CreateAudio(audio, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffSampleTableBox CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffSampleTableBox(PiffSampleDescriptionBox.CreateVideo(video, keyId));
        }

        #endregion
    }
}