using System.Collections.Generic;
using System.Linq;


namespace PiffLibrary
{
    /// <summary>
    /// Location and presentation time of sync samples.
    /// Not every sync sample needs to be listed.
    /// </summary>
    [BoxName("tfra")]
    internal class PiffTrackFragmentRandomAccess : PiffFullBoxBase
    {
        #region Properties

        public uint TrackId { get; set; }


        /// <summary>
        /// 26 bits - reserved (0)
        /// 2 bits - length of <see cref="PiffSampleOffsetItem.TrafNumber"/> - 1 [bytes]
        /// 2 bits - length of <see cref="PiffSampleOffsetItem.TrunNumber"/> - 1 [bytes]
        /// 2 bits - length of <see cref="PiffSampleOffsetItem.SampleNumber"/> - 1 [bytes]
        /// </summary>
        public int Lengths { get; set; }


        /// <summary>
        /// The number of elements in <see cref="Offsets"/>.
        /// May be 0, then every sample is a sync sample.
        /// </summary>
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffSampleOffsetItem[] Offsets { get; set; }

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
        public PiffTrackFragmentRandomAccess(uint trackId, IEnumerable<PiffSampleOffsetDto> offsets)
        {
            Version = 1;
            TrackId = trackId;
            Offsets = (from off in offsets select new PiffSampleOffsetItem(this)
            {
                Time = off.Time,
                Offset = off.Offset,
                TrafNumber = new byte[] { off.TrafNumber },
                TrunNumber = new byte[] { off.TrunNumber },
                SampleNumber = new byte[] { off.SampleNumber }
            }).ToArray();
            Count = (uint)Childen.Length;
        }

        #endregion


        #region API

        internal int GetTrafNumberSize() => ((Lengths & 0b110000) >> 4) + 1;


        internal int GetTrunNumberSize() => ((Lengths & 0b001100) >> 2) + 1;


        internal int GetSampleNumberSize() => (Lengths & 0b000011) + 1;

        #endregion
    }
}