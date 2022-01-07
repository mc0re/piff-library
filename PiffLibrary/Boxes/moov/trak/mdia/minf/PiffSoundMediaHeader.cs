namespace PiffLibrary
{
    [BoxName("smhd")]
    internal class PiffSoundMediaHeader : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Stereo position of the mono track, 8.8 fixed point.
        /// 0.0 is center, -1.0 - left.
        /// </summary>
        public short Balance { get; set; }


        public short Reserved1 { get; set; }


        #endregion
    }
}