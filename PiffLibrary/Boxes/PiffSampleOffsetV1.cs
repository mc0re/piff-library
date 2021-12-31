namespace PiffLibrary
{
    internal class PiffSampleOffsetV1
    {
        #region Properties

        /// <summary>
        /// Explicit parent.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffTrackFragmentRandomAccess Parent { get; }


        /// <summary>
        /// Start time of a "moof" box.
        /// </summary>
        public long Time { get; set; }


        /// <summary>
        /// Offset of the that "moof" block from the beginning of the file.
        /// </summary>
        public long Offset { get; set; }


        /// <summary>
        /// Length is "length of TrafNumber field" + 1 bytes.
        /// The number of "traf" box containing the sync sample. Starts with 1 in each "moof".
        /// </summary>
        public byte TrafNumber { get; set; } = 1;


        /// <summary>
        /// Length is "length of TrunNumber field" + 1 bytes.
        /// The number of "trun" box containing the sync sample. Starts with 1 in each "traf".
        /// </summary>
        public byte TrunNumber { get; set; } = 1;


        /// <summary>
        /// Length is "length of SampleNumber field" + 1 bytes.
        /// The number of the sync sample. Starts with 1 in each "trun".
        /// </summary>
        public byte SampleNumber { get; set; } = 1;

        #endregion


        #region Init and clean-up

        public PiffSampleOffsetV1(PiffTrackFragmentRandomAccess parent)
        {
            Parent = parent;
        }

        #endregion


        #region Format API

        public PiffDataFormats GetDateFormat()
        {
            return Parent.Version == 0 ? PiffDataFormats.Int32 : PiffDataFormats.Int64;
        }

        #endregion
    }
}
