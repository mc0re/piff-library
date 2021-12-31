using System;

namespace PiffLibrary
{
    [BoxName("frma")]
    internal class PiffProtectionOriginalFormat : PiffBoxBase
    {
        #region Properties

        public string Format { get; }

        #endregion


        #region Init and clean-up

        private PiffProtectionOriginalFormat(string format)
        {
            Format = format;
        }


        public static PiffProtectionOriginalFormat CreateAudio(string codecId)
        {
            if (codecId != "AACL")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffProtectionOriginalFormat("mp4a");
        }


        public static PiffProtectionOriginalFormat CreateVideo(string codecId)
        {
            if (codecId != "H264")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffProtectionOriginalFormat("avc1");
        }

        #endregion
    }
}