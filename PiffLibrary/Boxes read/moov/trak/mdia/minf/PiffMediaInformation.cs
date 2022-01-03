using System;

namespace PiffLibrary
{
    [BoxName("minf")]
    internal class PiffMediaInformation : PiffBoxBase
    {
        #region Properties

        public PiffSoundMediaHeader SoundHeader { get; set; }

        public PiffVideoMediaHeader VideoHeader { get; set; }

        public PiffDataInformation Info { get; set; } = new PiffDataInformation();

        public PiffSampleTable Index { get; set; }

        #endregion


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
            SoundHeader = sound;
            VideoHeader = video;
            Index = index;
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