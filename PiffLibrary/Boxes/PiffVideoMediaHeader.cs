namespace PiffLibrary
{
    [BoxName("vmhd")]
    internal class PiffVideoMediaHeader : PiffBoxBase
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 1;


        /// <summary>
        /// Only mode 0 is currently defined.
        /// 0 = copy
        /// </summary>
        public short GraphicsMode { get; } = 0;


        /// <summary>
        /// RGB colour available for other future graphics modes.
        /// </summary>
        public short[] Color { get; } = { 0, 0, 0 };

        #endregion
    }
}