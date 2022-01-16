namespace PiffLibrary.Boxes
{
    public sealed class PiffSampleEncryptionSubSample
    {
        /// <summary>
        /// The number of bytes of clear data at the beginning of this subsample. May be 0.
        /// </summary>
        public ushort ClearDataSize { get; set; }


        /// <summary>
        /// The number of bytes of encrypted data after the clear data. May be 0.
        /// </summary>
        public uint EncryptedDataSize { get; set; }
    }
}