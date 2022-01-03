﻿namespace PiffLibrary
{
    [BoxName("stsc")]
    internal class PiffSampleToChunk : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        /// <summary>
        /// The number of elements in the following array.
        /// </summary>
        public int Count { get; set; } = 0;

        #endregion
    }
}