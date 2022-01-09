namespace PiffLibrary
{
    internal class PiffTrackFragmentRunSample
    {
        #region Properties

        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffTrackFragmentRun Parent { get; }


        [PiffDataFormat(nameof(FlagsHaveDuration))]
        public uint SampleDuration { get; set; }


        [PiffDataFormat(nameof(FlagsHaveSize))]
        public uint SampleSize { get; set; }


        [PiffDataFormat(nameof(FlagsHaveFlags))]
        public uint SampleFlags { get; set; }


        [PiffDataFormat(nameof(GetOffsetFormat))]
        public long SampleOffset { get; set; }

        #endregion


        #region Init and clean-up

        public PiffTrackFragmentRunSample(PiffTrackFragmentRun parent)
        {
            Parent = parent;
        }

        #endregion


        #region Format API

        public PiffDataFormats FlagsHaveDuration() =>
            (Parent.Flags & 0x100) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveSize() =>
            (Parent.Flags & 0x200) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveFlags() =>
            (Parent.Flags & 0x400) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;


        /// <summary>
        /// Version = 0 - unsigned, otherwise signed.
        /// </summary>
        public PiffDataFormats GetOffsetFormat() =>
            (Parent.Flags & 0x800) == 0 ? PiffDataFormats.Skip :
            Parent.Version == 0 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Int32;

        #endregion
    }
}