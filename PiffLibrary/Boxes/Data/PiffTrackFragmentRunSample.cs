namespace PiffLibrary.Boxes
{
    public sealed class PiffTrackFragmentRunSample
    {
        #region Constants

        public const int FlagsSampleIsDifference = 0x10000;

        #endregion


        #region Properties

        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffTrackFragmentRunBox Parent { get; }


        [PiffDataFormat(nameof(FlagsHaveDuration))]
        public uint Duration { get; set; }


        [PiffDataFormat(nameof(FlagsHaveSize))]
        public uint Size { get; set; }


        [PiffDataFormat(nameof(FlagsHaveFlags))]
        public uint Flags { get; set; }


        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long TimeOffset { get; set; }

        #endregion


        #region Init and clean-up

        public PiffTrackFragmentRunSample(PiffTrackFragmentRunBox parent)
        {
            Parent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats FlagsHaveDuration() =>
            (Parent.Flags & PiffTrackFragmentRunBox.FlagsSampleDurationPresent) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveSize() =>
            (Parent.Flags & PiffTrackFragmentRunBox.FlagsSampleSizePresent) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        private PiffDataFormats FlagsHaveFlags() =>
            (Parent.Flags & PiffTrackFragmentRunBox.FlagsSampleFlagsPresent) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        /// <summary>
        /// Version = 0 - unsigned, otherwise signed.
        /// </summary>
        private PiffDataFormats GetOffsetFormat() =>
            (Parent.Flags & PiffTrackFragmentRunBox.FlagsTimeOffsetPresent) == 0 ? PiffDataFormats.Skip :
            Parent.Version == 0 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Int32;

        #endregion
    }
}