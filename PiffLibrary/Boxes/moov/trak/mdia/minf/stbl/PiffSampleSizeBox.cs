﻿namespace PiffLibrary.Boxes
{
    [BoxName("stsz")]
    public sealed class PiffSampleSizeBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of bytes in a single sample valid for all samples.
        /// </summary>
        public uint SampleSize { get; set; }


        /// <summary>
        /// The number of samples in the track.
        /// </summary>
        public uint SampleCount { get; set; }


        /// <summary>
        /// Individual sizes for each sample in the track.
        /// </summary>
        [PiffArraySize(nameof(NofEntries))]
        public uint[] EntrySizes { get; set; }

        #endregion


        #region Format API

        private uint NofEntries => SampleSize == 0 ? SampleCount : 0;

        #endregion
    }
}