namespace PiffLibrary
{
    [BoxName("url ")]
    internal class PiffDataEntryUrl : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit time and duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; } = 0;


        /// <summary>
        /// Whether the data is in the same file as this box.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 1;

        #endregion
    }
}