using System;

namespace PiffLibrary.Boxes
{
    [BoxName("frma")]
    public sealed class PiffOriginalFormatBox : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// Format of the decrypted or un-transformed data.
        /// </summary>
        [PiffStringLength(4)]
        public string Format { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffOriginalFormatBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffOriginalFormatBox(string format)
        {
            Format = format;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffOriginalFormatBox CreateAudio(string codecId)
        {
            if (codecId != "AACL")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffOriginalFormatBox("mp4a");
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffOriginalFormatBox CreateVideo(string codecId)
        {
            if (codecId != "H264")
                throw new ArgumentException($"Cannot convert codec type '{codecId}'.");

            return new PiffOriginalFormatBox("avc1");
        }

        #endregion
    }
}