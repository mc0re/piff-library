﻿namespace PiffLibrary.Boxes
{
    public sealed class PiffSampleGroupDescriptionItem
    {
        #region Fields

        private PiffSampleGroupDescriptionBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// Length of this entry when <see cref="PiffSampleGroupDescriptionBox.DefaultLength"/> is 0.
        /// </summary>
        [PiffDataFormat(nameof(UseLength))]
        public uint DescriptionLength { get; set; }


        /// <summary>
        /// At the moment of writing this code, neither the mapping between
        /// grouping type and entry type, nor the entry type data are defined by ISO.
        /// 
        /// Mentioned types:
        /// - Visual
        /// - Audio
        /// - Hint
        /// - Subtitle
        /// - Text
        /// </summary>
        [PiffArraySize(nameof(EntrySize))]
        public byte[] Entry { get; set; }

        #endregion


        #region Init and clean-up

        public PiffSampleGroupDescriptionItem(PiffSampleGroupDescriptionBox parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats UseLength() =>
            mParent.Version == 1 && mParent.DefaultLength == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private uint EntrySize => mParent.DefaultLength > 0 ? mParent.DefaultLength : DescriptionLength;

        #endregion
    }
}
