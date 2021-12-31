using System.Collections.Generic;
using System.Linq;


namespace PiffLibrary
{
    [BoxName("tfra")]
    internal class PiffTrackFragmentRandomAccess : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit time and duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; } = 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        public int TrackId { get; }


        /// <summary>
        /// 26 bits = 0
        /// 2 bits - length of TrafNumber field - 1 [bytes]
        /// 2 bits - length of TrunNumber field - 1 [bytes]
        /// 2 bits - length of SampleNumber field - 1 [bytes]
        /// </summary>
        public int Reserved { get; } = 0;


        /// <summary>
        /// The number of elements in the array below.
        /// </summary>
        public int Count { get; }


        [PiffDataFormat(PiffDataFormats.InlineObject)]
        public PiffSampleOffsetV1[] Offsets { get; }

        #endregion


        #region Init and clean-up

        public PiffTrackFragmentRandomAccess(int trackId, IEnumerable<PiffSampleOffsetV1> offsets)
        {
            TrackId = trackId;
            Offsets = offsets.ToArray();
            Count = Offsets.Length;
        }

        #endregion


        #region API

        public int GetLength()
        {
            return 24 + Count * 19 /* size of PiffSampleOffsetV1 */;
        }

        #endregion
    }


    public class PiffSampleOffsetV1
    {
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
        public byte TrafNumber { get; } = 1;


        /// <summary>
        /// Length is "length of TrunNumber field" + 1 bytes.
        /// The number of "trun" box containing the sync sample. Starts with 1 in each "traf".
        /// </summary>
        public byte TrunNumber { get; } = 1;


        /// <summary>
        /// Length is "length of SampleNumber field" + 1 bytes.
        /// The number of the sync sample. Starts with 1 in each "trun".
        /// </summary>
        public byte SampleNumber { get; } = 1;
    }
}