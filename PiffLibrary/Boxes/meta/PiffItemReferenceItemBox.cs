namespace PiffLibrary.Boxes
{
    /// <summary>
    /// A single reference block (one-to-many).
    /// </summary>
    [BoxName("hint")]
    [BoxName("cdsc")]
    [BoxName("font")]
    [BoxName("hind")]
    [BoxName("vdep")]
    [BoxName("vplx")]
    [BoxName("subt")]
    public class PiffItemReferenceItemBox : PiffBoxBase
    {
        #region Fields

        private readonly PiffItemReferenceBox mParent;

        #endregion


        #region Properties

        /// <summary>
        /// ID of the item that refers to other items.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        public ulong FromItemId { get; set; }


        public ushort Count { get; set; }


        /// <summary>
        /// IDs the item refers to.
        /// </summary>
        [PiffDataFormat(nameof(GetIdFormat))]
        [PiffArraySize(nameof(Count))]
        public ulong[] ToItemIds { get; set; }

        #endregion


        #region Init and clean-up

        public PiffItemReferenceItemBox(PiffItemReferenceBox parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetIdFormat() =>
            mParent.Version == 0 ? PiffDataFormats.UInt16 : PiffDataFormats.UInt32;

        #endregion
    }
}
