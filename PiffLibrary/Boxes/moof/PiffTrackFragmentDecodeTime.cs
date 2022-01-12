using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("tfdt")]
    internal class PiffTrackFragmentDecodeTime : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Absolute decode time (in media timeline, expressed in media time scale units)
        /// of the first sample in decode order in the enclosing track fragment.
        /// </summary>
        [PiffDataFormat(nameof(GetTimeFormat))]
        public ulong DecodeTime { get; set; }

        #endregion


        #region Format API

        public PiffDataFormats GetTimeFormat() =>
            Version == 0 ? PiffDataFormats.UInt32 : PiffDataFormats.UInt64;

        #endregion
    }
}