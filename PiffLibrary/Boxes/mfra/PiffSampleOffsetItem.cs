namespace PiffLibrary
{
    internal class PiffSampleOffsetItem
    {
        #region Fields

        private readonly PiffTrackFragmentRandomAccess mParent;

        #endregion


        #region Properties

        /// <summary>
        /// Start time of a "moof" box, in time scale units.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public ulong Time { get; set; }


        /// <summary>
        /// Offset of the that "moof" block from the beginning of the file.
        /// </summary>
        [PiffDataFormat(nameof(GetOffsetFormat))]
        public ulong Offset { get; set; }


        /// <summary>
        /// Length is "length of TrafNumber field" + 1 bytes.
        /// The number of "traf" box containing the sync sample. Starts with 1 in each "moof".
        /// </summary>
        [PiffArraySize(nameof(TrafNumberLength))]
        public byte[] TrafNumber { get; set; }


        /// <summary>
        /// Length is "length of TrunNumber field" + 1 bytes.
        /// The number of "trun" box containing the sync sample. Starts with 1 in each "traf".
        /// </summary>
        [PiffArraySize(nameof(TrunNumberLength))]
        public byte[] TrunNumber { get; set; }


        /// <summary>
        /// Length is "length of SampleNumber field" + 1 bytes.
        /// The number of the sync sample. Starts with 1 in each "trun".
        /// </summary>
        [PiffArraySize(nameof(SampleNumberLength))]
        public byte[] SampleNumber { get; set; }

        #endregion


        #region Init and clean-up

        public PiffSampleOffsetItem(PiffTrackFragmentRandomAccess parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        public PiffDataFormats GetDateFormat()
        {
            return mParent.Version != 1 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;
        }


        public PiffDataFormats GetOffsetFormat()
        {
            return mParent.Version != 1 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;
        }


        [PiffDataFormat(PiffDataFormats.Skip)]
        public int TrafNumberLength => mParent.GetTrafNumberSize();


        [PiffDataFormat(PiffDataFormats.Skip)]
        public int TrunNumberLength => mParent.GetTrunNumberSize();


        [PiffDataFormat(PiffDataFormats.Skip)]
        public int SampleNumberLength => mParent.GetSampleNumberSize();

        #endregion
    }
}
