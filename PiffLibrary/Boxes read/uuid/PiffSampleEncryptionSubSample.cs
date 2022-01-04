namespace PiffLibrary
{
    internal class PiffSampleEncryptionSubSample
    {
        /// <summary>
        /// The number of bytes of clear data at the beginning of this subsample. May be 0.
        /// </summary>
        public short ClearDataSize { get; set; }


        /// <summary>
        /// The number of bytes of encrypted data after the clear data. May be 0.
        /// </summary>
        public int EncryptedDataSize { get; set; }
    }
}