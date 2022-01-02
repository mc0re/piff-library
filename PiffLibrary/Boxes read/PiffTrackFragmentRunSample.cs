namespace PiffLibrary
{
    internal class PiffTrackFragmentRunSample
    {
        #region Properties

        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffTrackFragmentRun Parent { get; }


        [PiffDataFormat(nameof(FlagsHaveDuration))]
        public int SampleDuration { get; set; }


        [PiffDataFormat(nameof(FlagsHaveSize))]
        public int SampleSize { get; set; }


        [PiffDataFormat(nameof(FlagsHaveFlags))]
        public int SampleFlags { get; set; }


        [PiffDataFormat(nameof(FlagsHaveOffset))]
        public int SampleOffset { get; set; }

        #endregion


        #region Init and clean-up

        public PiffTrackFragmentRunSample(PiffTrackFragmentRun parent)
        {
            Parent = parent;
        }

        #endregion


        #region Format API

        public PiffDataFormats FlagsHaveDuration() =>
            (Parent.Flags & 0x100) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveSize() =>
            (Parent.Flags & 0x200) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        public PiffDataFormats FlagsHaveFlags() =>
            (Parent.Flags & 0x400) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;


        /// <summary>
        /// Version = 0 - unsigned, otherwise signed.
        /// </summary>
        public PiffDataFormats FlagsHaveOffset() =>
            (Parent.Flags & 0x800) != 0 ? PiffDataFormats.Int32 : PiffDataFormats.Skip;

        #endregion
    }
}