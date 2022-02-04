namespace PiffLibrary.Boxes
{
    public sealed class PiffDecoderSpecificInfoDescriptor
    {
        #region Fields

        private PiffDescriptorContainer mParent;

        #endregion


        #region Properties

        /// <summary>
        /// Codec data.
        /// </summary>
        [PiffArraySize(nameof(DsiDataSize))]
        public byte[] DsiData { get; set; }

        #endregion


        #region Init and clean-up

        public PiffDecoderSpecificInfoDescriptor(PiffDescriptorContainer parent)
        {
            mParent = parent;
        }

        #endregion


        #region Format

        private int DsiDataSize => mParent.Length;

        #endregion
    }
}