namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Flags:
    /// 
    /// 0x10000 - Duration is empty (there are no samples)
    /// 0x20000 - Default base is <see cref="PiffMovieFragmentBox"/>.
    ///           if <see cref="BaseDataOffset"/> is set,
    ///           this flag is ignored. If not, this indicates that the
    ///           base‐data‐offset for this track fragment is the position of the first byte
    ///           of the enclosing <see cref="PiffMovieFragmentBox"/> box.
    /// </summary>
    [BoxName("tfhd")]
    public sealed class PiffTrackFragmentHeaderBox : PiffFullBoxBase
    {
        #region Constants

        public const int FlagsDescriptionIndexPresent = 2;

        #endregion


        #region Properties

        /// <summary>
        /// Which track ID this data corresponds to.
        /// </summary>
        public uint TrackId { get; set; }


        /// <summary>
        /// Identical to the chunk offset in <see cref="PiffChunkOffsetBox"/>.
        /// If not, the offsets need to be established relative to the movie fragment.
        /// </summary>
        [PiffDataFormat(nameof(FlagsHaveBaseOffset))]
        public ulong BaseDataOffset { get; set; }


        /// <summary>
        /// Overrid for this fragment the default from <see cref="PiffTrackExtendedBox"/>.
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

        private PiffDataFormats FlagsHaveBaseOffset() =>
            (Flags & 1) != 0 ? PiffDataFormats.UInt64 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveIndex() =>
            (Flags & FlagsDescriptionIndexPresent) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveDuration() =>
            (Flags & 8) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveSize() =>
            (Flags & 0x10) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveFlags() =>
            (Flags & 0x20) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}