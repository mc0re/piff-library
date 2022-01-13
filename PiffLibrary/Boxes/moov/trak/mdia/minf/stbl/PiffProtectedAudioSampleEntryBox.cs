using System;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// This is AudioSampleEntry, not AudioSampleEntryV1.
    /// </summary>
    [BoxName("enca")]
    [ChildType(typeof(PiffProtectionSchemeInformationBox))]
    [ChildType(typeof(PiffElementaryStreamDescriptionMp4aBox))] // For WMA streams a "wfex" block comes instead
    public sealed class PiffProtectedAudioSampleEntryBox : PiffBoxBase
    {
        #region Constants

        private const int BufferSize = 6144;

        #endregion


        #region Properties

        [PiffArraySize(6)]
        public byte[] Reserved1 { get; set; } = new byte[] { 0, 0, 0, 0, 0, 0 };


        /// <summary>
        /// Index to the data reference.
        /// </summary>
        public ushort DataReferenceIndex { get; set; } = 1;


        public short Version { get; set; }


        [PiffArraySize(3)]
        public short[] Reserved2 { get; set; } = { 0, 0, 0 };


        public short ChannelCount { get; set; }


        public short SampleSize { get; set; }


        [PiffArraySize(2)]
        public short[] Reserved3 { get; set; } = { 0, 0 };


        public uint SampleRate { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectedAudioSampleEntryBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffProtectedAudioSampleEntryBox(PiffAudioManifest audio, Guid keyId)
        {
            if (audio.Channels != 2)
                throw new ArgumentException("AudioSampleEntry must have 2 channels.");

            if (audio.CodecId != "AACL")
                throw new ArgumentException($"Cannot process codec '{audio.CodecId}', only 'AACL' is supported.");

            ChannelCount = audio.Channels;
            SampleSize = audio.BitsPerSample;
            SampleRate = (uint) audio.SamplingRate << 16;
            var scheme = PiffProtectionSchemeInformationBox.CreateAudio(audio.CodecId, keyId);
            var desc = PiffElementaryStreamDescriptionMp4aBox.Create(
                audio.CodecId, 0, audio.BitRate, BufferSize, audio.CodecData);
            Childen = new PiffBoxBase[] { scheme, desc };
        }

        #endregion
    }
}