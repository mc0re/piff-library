using System;
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

        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffTrackFragmentRandomAccess(int trackId, IEnumerable<PiffSampleOffset> offsets)
        {
            TrackId = trackId;
            Offsets = (from off in offsets select new PiffSampleOffsetV1(this)
            {
                Time = off.Time,
                Offset = off.Offset,
                TrafNumber = off.TrafNumber,
                TrunNumber = off.TrunNumber,
                SampleNumber = off.SampleNumber
            }).ToArray();
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
}