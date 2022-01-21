using System;


namespace PiffLibrary.Boxes
{
    [BoxName("enca")]
    [ChildType(typeof(PiffProtectionSchemeInformationBox))]
    [ChildType(typeof(PiffElementaryStreamDescriptionMp4aBox))] // For WMA streams a "wfex" block comes instead
    [ChildType(typeof(PiffChannelLayoutBox))]
    [ChildType(typeof(PiffSamplingRateBox))] // For V1 only
    public sealed class PiffAudioSampleEntryBox : PiffSampleEntryBoxBase
    {
        #region Constants

        private const int BufferSize = 6144;

        #endregion


        #region Properties

        /// <summary>
        /// Defines V0 or V1 type of box.
        /// </summary>
        public ushort Version { get; set; }


        [PiffArraySize(3)]
        public short[] Reserved2 { get; } = { 0, 0, 0 };


        /// <summary>
        /// 1 - mono
        /// 2 - stereo
        /// other - the codec configuration shall identify the channel assignment
        /// </summary>
        public ushort ChannelCount { get; set; }


        /// <summary>
        /// Bits per sample, default 16.
        /// </summary>
        public ushort SampleSize { get; set; }


        [PiffArraySize(2)]
        public short[] Reserved3 { get; } = { 0, 0 };


        /// <summary>
        /// When a <see cref="PiffSamplingRateBox"/> is absent is the sampling rate;
        /// when a <see cref="PiffSamplingRateBox"/> is present, is a suitable
        /// integer multiple or division of the actual sampling rate.
        /// Fixed point 16.16.
        /// </summary>
        public uint SampleRate { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffAudioSampleEntryBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffAudioSampleEntryBox(PiffAudioManifest audio, Guid keyId)
        {
            if (audio.Channels != 2)
                throw new ArgumentException("AudioSampleEntry must have 2 channels.");

            if (audio.CodecId != "AACL")
                throw new ArgumentException($"Cannot process codec '{audio.CodecId}', only 'AACL' is supported.");

            DataReferenceIndex = 1;
            ChannelCount = audio.Channels;
            SampleSize = audio.BitsPerSample;
            SampleRate = (uint) audio.SamplingRate << 16;
            var scheme = PiffProtectionSchemeInformationBox.CreateAudio(audio.CodecId, keyId);
            var desc = PiffElementaryStreamDescriptionMp4aBox.Create(
                audio.CodecId, 0, audio.BitRate, BufferSize, audio.CodecData);
            Childen = new PiffBoxBase[] { desc, scheme };
        }

        #endregion
    }
}