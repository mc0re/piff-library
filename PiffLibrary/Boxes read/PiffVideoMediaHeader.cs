namespace PiffLibrary
{
    [BoxName("vmhd")]
    internal class PiffVideoMediaHeader : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 1;


        /// <summary>
        /// Only mode 0 is currently defined.
        /// 0 = copy
        /// </summary>
        public short GraphicsMode { get; set; } = 0;


        /// <summary>
        /// RGB colour available for other future graphics modes.
        /// </summary>
        [PiffArraySize(3)]
        public short[] Color { get; set; } = { 0, 0, 0 };

        #endregion
    }
}