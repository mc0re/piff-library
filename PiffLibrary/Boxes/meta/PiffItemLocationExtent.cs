namespace PiffLibrary.Boxes
{
    public class PiffItemLocationExtent
    {
        #region Fields

        private readonly PiffItemLocationResource mParent;

        #endregion


        #region Properties

        /// <summary>
        /// If expected by the given <see cref="PiffItemLocationResource.ConstructionMethod"/>.
        /// </summary>
        [PiffDataFormat(nameof(GetIndexFormat))]
        public ulong Index { get; set; }


        /// <summary>
        /// Absolute offset from the data origin of the container [bytes].
        /// </summary>
        [PiffDataFormat(nameof(GetOffsetFormat))]
        public ulong Offset { get; set; }


        /// <summary>
        /// Length in bytes of this extent.
        /// If 0, the length of the entire referenced container is implied.
        /// </summary>
        [PiffDataFormat(nameof(GetLengthFormat))]
        public ulong Length { get; set; }

        #endregion


        #region Init and clean-up

        public PiffItemLocationExtent(PiffItemLocationResource parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format API

        private PiffDataFormats GetIndexFormat() =>
            mParent.Parent.Version != 1 && mParent.Parent.Version != 2 ? PiffDataFormats.Skip :
            mParent.Parent.IndexSize == 8 ? PiffDataFormats.UInt64 :
            mParent.Parent.IndexSize == 4 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Skip;


        private PiffDataFormats GetOffsetFormat() =>
            mParent.Parent.OffsetSize == 8 ? PiffDataFormats.UInt64 :
            mParent.Parent.OffsetSize == 4 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Skip;


        private PiffDataFormats GetLengthFormat() =>
            mParent.Parent.LengthSize == 8 ? PiffDataFormats.UInt64 :
            mParent.Parent.LengthSize == 4 ? PiffDataFormats.UInt32 :
            PiffDataFormats.Skip;

        #endregion
    }
}
