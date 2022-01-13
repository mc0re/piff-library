using System;

namespace PiffLibrary.Boxes
{
    [BoxName("frma")]
    public sealed class PiffProtectionOriginalFormatBox : PiffBoxBase
    {
        #region Properties

        [PiffStringLength(4)]
        public string Format { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionOriginalFormatBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffProtectionOriginalFormatBox(string format)
        {
            Format = format;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionOriginalFormatBox CreateAudio(string codecId)
        {
            if (codecId != "AACL")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffProtectionOriginalFormatBox("mp4a");
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionOriginalFormatBox CreateVideo(string codecId)
        {
            if (codecId != "H264")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffProtectionOriginalFormatBox("avc1");
        }

        #endregion
    }
}