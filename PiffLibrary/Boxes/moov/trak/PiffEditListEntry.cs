namespace PiffLibrary.Boxes
{
    public sealed class PiffEditListEntry
    {
        #region Fields

        /// <summary>
        /// Explicit parent.
        /// </summary>
        private readonly PiffEditListBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// Duration of this edit segment in units of the timescale in the Movie Header Box.
        /// </summary>
        [PiffDataFormat(nameof(GetDurationFormat))]
        public ulong SegmentDuration { get; set; }


        /// <summary>
        /// Starting time within the media of this edit segment (in media time scale units, in composition time).
        /// If –1, it is an empty edit.
        /// </summary>
        [PiffDataFormat(nameof(GetTimeFormat))]
        public long MediaTime { get; set; }


        /// <summary>
        /// Relative rate at which to play the media corresponding to this edit segment.
        /// Fixed point number, format is 16.16.
        /// Allowed values are 0.0 ("dwell") and 1.0.
        /// </summary>
        public int MediaRate { get; set; }

        #endregion


        #region Init and clean-up

        public PiffEditListEntry(PiffEditListBox parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetDurationFormat() =>
            mParent.Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;


        private PiffDataFormats GetTimeFormat() =>
            mParent.Version == 0 ? PiffDataFormats.Int32 : PiffDataFormats.Int64;

        #endregion
    }
}
