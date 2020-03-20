﻿using System;

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


        public static PiffMediaInformation CreateAudio(short trackId, PiffAudioManifest audio, Guid keyId)
        {
            return new PiffMediaInformation(new PiffSoundMediaHeader(),
                                            null,
                                            PiffSampleTable.CreateAudio(trackId, audio, keyId));
        }


        public static PiffMediaInformation CreateVideo(short width, short height, Guid keyId)
        {
            return new PiffMediaInformation(null, new PiffVideoMediaHeader(),
                                            PiffSampleTable.CreateVideo(width, height, keyId));
        }

        #endregion
    }
}