using System;

namespace PiffLibrary
{
    [BoxName("minf")]
    internal class PiffMediaInformation
    {
        #region Properties

        public PiffSoundMediaHeader SoundHeader { get; }

        public PiffVideoMediaHeader VideoHeader { get; }

        public PiffDataInformation Info { get; } = new PiffDataInformation();

        public PiffSampleTable Index { get; }

        #endregion


        #region Init and clean-up

        private PiffMediaInformation(PiffSoundMediaHeader sound, PiffVideoMediaHeader video, PiffSampleTable index)
        {
            SoundHeader = sound;
            VideoHeader = video;
            Index = index;
        }


        public static PiffMediaInformation CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffMediaInformation(new PiffSoundMediaHeader(),
                                            null,
                                            PiffSampleTable.CreateAudio(audio, keyId));
        }


        public static PiffMediaInformation CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffMediaInformation(null, new PiffVideoMediaHeader(),
                                            PiffSampleTable.CreateVideo(video, keyId));
        }

        #endregion
    }
}