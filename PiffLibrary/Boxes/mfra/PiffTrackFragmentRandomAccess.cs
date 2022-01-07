using System.Collections.Generic;
using System.Linq;


namespace PiffLibrary
{
    [BoxName("tfra")]
    internal class PiffTrackFragmentRandomAccess : PiffFullBoxBase
    {
        #region Properties

        public uint TrackId { get; set; }


        /// <summary>
        /// 26 bits = 0
        /// 2 bits - length of TrafNumber field - 1 [bytes]
        /// 2 bits - length of TrunNumber field - 1 [bytes]
        /// 2 bits - length of SampleNumber field - 1 [bytes]
        /// </summary>
        public int Reserved { get; set; }


        /// <summary>
        /// The number of elements in <see cref="Offsets"/>.
        /// </summary>
        public int Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSampleOffsetV1[] Offsets { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffTrackFragmentRandomAccess()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffTrackFragmentRandomAccess(uint trackId, IEnumerable<PiffSampleOffset> offsets)
        {
            Version = 1;
            TrackId = trackId;
            Offsets = (from off in offsets select new PiffSampleOffsetV1(this)
            {
                Time = off.Time,
                Offset = off.Offset,
                TrafNumber = off.TrafNumber,
                TrunNumber = off.TrunNumber,
                SampleNumber = off.SampleNumber
            }).ToArray();
            Count = Childen.Length;
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