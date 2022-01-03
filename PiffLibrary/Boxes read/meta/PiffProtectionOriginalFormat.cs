using System;

namespace PiffLibrary
{
    [BoxName("frma")]
    internal class PiffProtectionOriginalFormat : PiffBoxBase
    {
        #region Properties

        [PiffDataLength(4)]
        public string Format { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionOriginalFormat()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffProtectionOriginalFormat(string format)
        {
            Format = format;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionOriginalFormat CreateAudio(string codecId)
        {
            if (codecId != "AACL")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffProtectionOriginalFormat("mp4a");
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionOriginalFormat CreateVideo(string codecId)
        {
            if (codecId != "H264")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffProtectionOriginalFormat("avc1");
        }

        #endregion
    }
}