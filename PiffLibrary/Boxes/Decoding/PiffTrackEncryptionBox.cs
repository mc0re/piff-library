namespace PiffLibrary.Boxes
{
    [BoxName("tenc")]
    public sealed class PiffTrackEncryptionBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Same data can be stored as this box or as an extension box.
        /// </summary>
        public PiffProtectionTrackEncryption Data { get; set; }

        #endregion
    }
}