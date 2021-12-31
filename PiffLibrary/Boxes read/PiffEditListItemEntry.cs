namespace PiffLibrary
{
    internal class PiffEditListItemEntry
    {
        #region Properties

        /// <summary>
        /// Explicit parent.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Skip)]
        public PiffEditListItem Parent { get; }


        /// <summary>
        /// Duration of this edit segment in units of the timescale in the Movie Header Box.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public long SegmentDuration { get; set; }


        /// <summary>
        /// Starting time within the media of this edit segment (in media time scale units, in composition time).
        /// If –1, it is an empty edit.
        /// </summary>
        [PiffDataFormat(nameof(GetDateFormat))]
        public long MediaTime { get; set; }


        /// <summary>
        /// Relative rate at which to play the media corresponding to this edit segment.
        /// Fixed point number, format is 16.16.
        /// Allowe values are 0.0 ("dwell") and 1.0.
        /// </summary>
        public int MediaRate { get; set; }

        #endregion


        #region Init and clean-up

        public PiffEditListItemEntry(PiffEditListItem parent)
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
