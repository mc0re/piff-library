namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Object descriptor type ID.
    /// 
    /// Other known tags:
    /// - 1 or 17 - Object descriptor
    /// - 2 or 16 - Initial object descriptor
    /// - 10 - IPMP descriptor pointer
    /// - 11 - IPMP descriptor
    /// - 14 - ES ID INC descriptor
    /// - 15 - ES ID REF descriptor
    /// </summary>
    internal static class PiffEsdsBlockIds
    {
        /// <summary>
        /// Elementary Stream Descriptor.
        /// </summary>
        public const byte Esd = 3;


        /// <summary>
        /// Decoder Config Descriptor.
        /// </summary>
        public const byte Dcd = 4;


        /// <summary>
        /// Decoder Specific Info.
        /// </summary>
        public const byte Dsi = 5;


        /// <summary>
        /// Sync Layer Config Descriptor.
        /// </summary>
        public const byte Slc = 6;
    }
}
