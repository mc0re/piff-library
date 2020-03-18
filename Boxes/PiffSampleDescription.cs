﻿using System;

namespace PiffLibrary
{
    [BoxName("stsd")]
    internal class PiffSampleDescription
    {
        #region Properties

        public byte Version { get; } = 0;

        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;

        /// <summary>
        /// The number of elements in the following array
        /// (implemented as two mutually exclusive properties).
        /// </summary>
        public int Count { get; } = 1;

        public PiffProtectedAudioSampleEntry AudioEntry { get; }

        public PiffProtectedVideoSampleEntry VideoEntry { get; }

        #endregion


        #region Init and clean-up

        private PiffSampleDescription(PiffProtectedAudioSampleEntry audio, PiffProtectedVideoSampleEntry video)
        {
            AudioEntry = audio;
            VideoEntry = video;
        }


        public static PiffSampleDescription CreateAudio(
            short channels, short bitsPerSample, ushort samplingRate,
            short streamId, int bitRate, byte[] codecData, Guid keyId)
        {
            return new PiffSampleDescription(
                new PiffProtectedAudioSampleEntry(channels, bitsPerSample, samplingRate, streamId, bitRate, codecData, keyId),
                null);
        }


        public static PiffSampleDescription CreateVideo(short width, short height, Guid keyId)
        {
            return new PiffSampleDescription(
                null,
                new PiffProtectedVideoSampleEntry(width, height, keyId));
        }

        #endregion
    }
}