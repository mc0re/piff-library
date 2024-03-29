﻿namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Index of each chunk into the containing file.
    /// </summary>
    [BoxName("stco")]
    public sealed class PiffChunkOffsetBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of chunks.
        /// </summary>
        public uint Count { get; set; }


        /// <summary>
        /// Offset of each chunk from the beginning of the file.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        public uint[] Offsets { get; set; }

        #endregion
    }
}