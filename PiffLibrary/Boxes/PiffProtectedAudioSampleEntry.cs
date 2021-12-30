using System;


namespace PiffLibrary
{
    /// <summary>
    /// This is AudioSampleEntry, not AudioSampleEntryV1.
    /// </summary>
    [BoxName("enca")]
    internal class PiffProtectedAudioSampleEntry
    {
        #region Constants

        private const int BufferSize = 6144;

        #endregion


        #region Properties

        public byte[] Reserved1 { get; } = new byte[] { 0, 0, 0, 0, 0, 0 };


        /// <summary>
        /// Index to the data reference.
        /// </summary>
        public short DataReferenceIndex { get; } = 1;


        public short Version { get; } = 0;


        public short[] Reserved2 { get; } = { 0, 0, 0 };


        public short ChannelCount { get; }


        public short SampleSize { get; }


        public short[] Reserved3 { get; } = { 0, 0 };


        public uint SampleRate { get; }


        /// <summary>
        /// This is for MP4A streams.
        /// For WMA streams a "wfex" block comes instead.
        /// </summary>
        public PiffElementaryStreamDescription StreamDescription { get; }


        public PiffProtectionSchemeInformation Scheme { get; }

        #endregion


        #region Init and clean-up

        public PiffProtectedAudioSampleEntry(PiffAudioManifest audio, Guid keyId)
        {
            if (audio.Channels != 2)
                throw new ArgumentException("AudioSampleEntry must have 2 channels.");

            if (audio.CodecId != "AACL")
                throw new ArgumentException($"Cannot process codec '{audio.CodecId}', only 'AACL' is supported.");

            ChannelCount = audio.Channels;
            SampleSize = audio.BitsPerSample;
            SampleRate = ((uint)audio.SamplingRate) << 16;
            Scheme = PiffProtectionSchemeInformation.CreateAudio(audio.CodecId, keyId);
            StreamDescription = PiffElementaryStreamDescription.Create(
                audio.CodecId, 0, audio.BitRate, BufferSize, audio.CodecData);
        }

        #endregion
    }
}