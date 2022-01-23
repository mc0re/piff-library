using System;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Defines encryption parameters a single sample.
    /// </summary>
    public sealed class PiffSampleEncryption
    {
        #region Constants

        public static readonly Guid BoxId = Guid.Parse("a2394f52-5a9b-4f14-a244-6c427c648df4");

        #endregion


        #region Fields

        [PiffDataFormat(PiffDataFormats.Skip)]
        public int ParentFlags { get; }

        #endregion


        #region Properties

        [PiffDataFormat(nameof(UseAlgorithmInfo))]
        public PiffSampleEncryptionAlgorithm Algorithm { get; set; }


        /// <summary>
        /// Corresponds to the number of samples in this track fragment.
        /// </summary>
        public uint SampleCount { get; set; }


        [PiffArraySize(nameof(SampleCount))]
        public PiffSampleEncryptionItem[] Items { get; set; }

        #endregion


        #region Init and clean-up

        public PiffSampleEncryption(PiffExtensionBox parent)
        {
            ParentFlags = parent.Flags;
        }


        public PiffSampleEncryption(PiffSampleEncryptionBox parent)
        {
            ParentFlags = parent.Flags;
        }

        #endregion


        #region Format API

        /// <summary>
        /// If bit 0 is not set, use parameters from <see cref="PiffProtectionTrackEncryption"/>.
        /// </summary>
        private PiffDataFormats UseAlgorithmInfo() =>
            (ParentFlags & 1) != 0 ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}