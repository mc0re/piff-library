namespace PiffLibrary.Boxes
{
    [BoxName("vmhd")]
    public sealed class PiffVideoMediaHeaderBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Only mode 0 is currently defined.
        /// 0 = copy
        /// </summary>
        public short GraphicsMode { get; set; }


        /// <summary>
        /// RGB colour available for other future graphics modes.
        /// </summary>
        [PiffArraySize(3)]
        public short[] Color { get; set; } = { 0, 0, 0 };

        #endregion


        #region Init and clean-up

        public PiffVideoMediaHeaderBox()
        {
            Flags = 1;
        }

        #endregion
    }
}