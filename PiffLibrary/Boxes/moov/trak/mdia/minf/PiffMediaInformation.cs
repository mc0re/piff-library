using System;

namespace PiffLibrary
{
    [BoxName("minf")]
    [ChildType(typeof(PiffNullMediaHeader))]
    [ChildType(typeof(PiffSampleTable))]
    [ChildType(typeof(PiffDataInformation))]

    [ChildType(typeof(PiffSoundMediaHeader))]
    [ChildType(typeof(PiffVideoMediaHeader))]
    internal class PiffMediaInformation : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMediaInformation()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffMediaInformation(PiffSoundMediaHeader sound, PiffVideoMediaHeader video, PiffSampleTable index)
        {
            Childen = new PiffBoxBase[] { sound, video, index };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffMediaInformation CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffMediaInformation(new PiffSoundMediaHeader(),
                                            null,
                                            PiffSampleTable.CreateAudio(audio, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffMediaInformation CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffMediaInformation(null, new PiffVideoMediaHeader(),
                                            PiffSampleTable.CreateVideo(video, keyId));
        }

        #endregion
    }
}