namespace PiffLibrary
{
    public class PiffSampleOffsetDto
    {
        #region Properties

        /// <summary>
        /// Start time of a "moof" box.
        /// </summary>
        public ulong Time { get; set; }


        /// <summary>
        /// Offset of the that "moof" block from the beginning of the file.
        /// </summary>
        public ulong Offset { get; set; }


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
    }
}
