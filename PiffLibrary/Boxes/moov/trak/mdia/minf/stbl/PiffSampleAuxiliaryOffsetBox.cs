﻿namespace PiffLibrary.Boxes
{
    [BoxName("saio")]
    public sealed class PiffSampleAuxiliaryOffsetBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Format of the auxiliary information.
        /// </summary>
        [PiffDataFormat(nameof(HasAuxInfo))]
        public uint AuxInfoType { get; set; }


        /// <summary>
        /// Depends on <see cref="AuxInfoType"/>.
        /// </summary>
        [PiffDataFormat(nameof(HasAuxInfo))]
        public uint AuxInfoTypeParameter { get; set; }


        /// <summary>
        /// Can be 1 or equal to the number of chunks or track fragment runs.
        /// </summary>
        public uint Count { get; set; }


        /// <summary>
        /// Position in the file of the sample auxiliary information
        /// for each chunk or track fragment run.
        /// 
        /// If the count is 1, all auxiliary information blocks are contignuous.
        /// 
        /// When in <see cref="PiffSampleTableBox"/>, the offsets are absolute
        /// from the begining of the file.
        /// In <see cref="PiffTrackFragmentBox"/>, this value is relative to the base offset established
        /// by <see cref="PiffTrackFragmentHeaderBox"/> box in the same track fragment.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        [PiffDataFormat(nameof(GetOffsetFormat))]
        public ulong[] Offset { get; set; }

        #endregion


        #region Format API

        private PiffDataFormats HasAuxInfo() =>
            (Flags & 1) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats GetOffsetFormat() =>
            Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;

        #endregion
    }
}