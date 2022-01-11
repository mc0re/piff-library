namespace PiffLibrary
{
    /// <summary>
    /// Flags:
    /// 
    /// 0x10000 - Duration is empty (there are no samples)
    /// 0x20000 - Default base is <see cref="PiffMovieFragment"/>.
    ///           if <see cref="BaseDataOffset"/> is set,
    ///           this flag is ignored. If not, this indicates that the
    ///           base‐data‐offset for this track fragment is the position of the first byte
    ///           of the enclosing <see cref="PiffMovieFragment"/> box.
    /// </summary>
    [BoxName("tfhd")]
    internal class PiffTrackFragmentHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Which track ID this data corresponds to.
        /// </summary>
        public uint TrackId { get; set; }


        /// <summary>
        /// Identical to the chunk offset in <see cref="PiffChunkOffset"/>.
        /// If not, the offsets need to be established relative to the movie fragment.
        /// </summary>
        [PiffDataFormat(nameof(FlagsHaveBaseOffset))]
        public ulong BaseDataOffset { get; set; }


        /// <summary>
        /// Overrid for this fragment the default from <see cref="PiffTrackExtended"/>.
        /// </summary>
        [PiffDataFormat(nameof(FlagsHaveIndex))]
        public uint SampleDescriptionIndex { get; set; }


        [PiffDataFormat(nameof(FlagsHaveDuration))]
        public uint DefaultSampleDuration { get; set; }


        [PiffDataFormat(nameof(FlagsHaveSize))]
        public uint DefaultSampleSize { get; set; }


        [PiffDataFormat(nameof(FlagsHaveFlags))]
        public uint DefaultSampleFlags { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats FlagsHaveBaseOffset() =>
            (Flags & 1) != 0 ? PiffDataFormats.UInt64 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveIndex() =>
            (Flags & 2) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveDuration() =>
            (Flags & 8) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveSize() =>
            (Flags & 0x10) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveFlags() =>
            (Flags & 0x20) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}