namespace PiffLibrary
{
    [BoxName("smhd")]
    internal class PiffSoundMediaHeader : PiffBoxBase
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        /// <summary>
        /// Stereo position of the mono track, 8.8 fixed point.
        /// 0.0 is center, -1.0 - left.
        /// </summary>
        public short Balance { get; } = 0;


        public short Reserved1 { get; } = 0;


        #endregion
    }
}