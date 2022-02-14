namespace PiffLibrary.Boxes
{
    public enum PiffEncryptionTypes
    {
        /// <summary>
        /// Not encrypted.
        /// </summary>
        NoEncryption = 0,

        /// <summary>
        /// AES 128-bit in CTR mode.
        /// </summary>
        AesCtr = 1,

        /// <summary>
        /// AES 128-bit in CBC mode.
        /// </summary>
        AesCbc = 2
    }
}
