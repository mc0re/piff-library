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


        public static PiffMediaInformation CreateAudio(
            short channels, short bitsPerSample, ushort samplingRate,
            short streamId, int bitRate, byte[] codecData, Guid keyId)
        {
            return new PiffMediaInformation(new PiffSoundMediaHeader(), null,
                PiffSampleTable.CreateAudio(channels, bitsPerSample, samplingRate,
                                            streamId, bitRate, codecData, keyId));
        }


        public static PiffMediaInformation CreateVideo(short width, short height, Guid keyId)
        {
            return new PiffMediaInformation(null, new PiffVideoMediaHeader(),
                                            PiffSampleTable.CreateVideo(width, height, keyId));
        }

        #endregion
    }
}