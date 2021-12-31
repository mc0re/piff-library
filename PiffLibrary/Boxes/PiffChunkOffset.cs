﻿namespace PiffLibrary
{
    [BoxName("stco")]
    internal class PiffChunkOffset : PiffBoxBase
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        /// <summary>
        /// The number of elements in the following array.
        /// </summary>
        public int Count { get; } = 0;

        #endregion
    }
}