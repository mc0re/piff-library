using System;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// DRC boxes can also be present, but are not defined by ISO 14496-12:
    /// - DRCCoefficientsBasic
    /// - DRCCoefficientsUniDRC
    /// - DRCInstructionsBasic
    /// - DRCInstructionsUniDRC
    /// </summary>
    [BoxName("enca")]
    [ChildType(typeof(PiffProtectionSchemeInformationBox))]
    [ChildType(typeof(PiffElementaryStreamDescriptionBox))] // For WMA streams a "wfex" block comes instead
    [ChildType(typeof(PiffChannelLayoutBox))]
    [ChildType(typeof(PiffSamplingRateBox))] // For V1 only
    [ChildType(typeof(PiffDownmixInstructionsBox))] 
    public sealed class PiffAudioSampleEntryBox : PiffSampleEntryBoxBase
    {
        #region Constants

        private const int BufferSize = 6144;

        #endregion


        #region Version property

        /// <summary>
        /// Defines V0 or V1 type of box.
        /// </summary>
        public ushort Version { get; set; }

        #endregion


        #region Properties for version 0

        public ushort QtRevision { get; set; }


        public uint QtVendor { get; set; }


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


        public ushort QtCompressionId { get; set; }


        public ushort QtPacketSize { get; set; }


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
            var desc = PiffElementaryStreamDescriptionBox.Create(
                audio.CodecId, 0, audio.BitRate, BufferSize, audio.CodecData);
            Children = new PiffBoxBase[] { desc, scheme };
        }

        #endregion
    }
}