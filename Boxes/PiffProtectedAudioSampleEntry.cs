using System;

namespace PiffLibrary
{
    /// <summary>
    /// This is AudioSampleEntry, not AudioSampleEntryV1.
    /// </summary>
    [BoxName("enca")]
    internal class PiffProtectedAudioSampleEntry
    {
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

        public PiffElementaryStreamDescription StreamDescription { get; }

        public PiffProtectionSchemeInformation Scheme { get; }

        #endregion


        #region Init and clean-up

        public PiffProtectedAudioSampleEntry(
            short channels, short bitsPerSample, ushort samplingRate,
            short streamId, int bitRate, byte[] codecData, Guid keyId)
        {
            if (channels != 2)
                throw new ArgumentException("AudioSampleEntry must have 2 channels.");

            ChannelCount = channels;
            SampleSize = bitsPerSample;
            SampleRate = ((uint)samplingRate) << 16;
            Scheme = PiffProtectionSchemeInformation.CreateAudio(keyId);
            StreamDescription = PiffElementaryStreamDescription.Create(streamId, bitRate, codecData);
        }

        #endregion
    }
}