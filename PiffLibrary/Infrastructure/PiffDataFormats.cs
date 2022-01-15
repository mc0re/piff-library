namespace PiffLibrary
{
    /// <summary>
    /// In which format shall the data be written to the file.
    /// </summary>
    internal enum PiffDataFormats
    {
        /// <summary>
        /// Do not write this value.
        /// </summary>
        Skip,

        /// <summary>
        /// 2 bits unsigned integer. The actual value is +1 (so 0b11 corresponds to 4).
        /// </summary>
        UInt2Minus1,

        /// <summary>
        /// 3 bits unsigned integer.
        /// </summary>
        UInt3,

        /// <summary>
        /// 4 bits unsigned integer.
        /// </summary>
        UInt4,

        /// <summary>
        /// 5 bits unsigned integer.
        /// </summary>
        UInt5,

        /// <summary>
        /// 6 bits unsigned integer.
        /// </summary>
        UInt6,

        /// <summary>
        /// 1-byte unsigned nteger. Default for <see langword="byte"/>.
        /// </summary>
        UInt8,

        /// <summary>
        /// 2-byte integer. Default for <see langword="short"/>.
        /// </summary>
        Int16,

        /// <summary>
        /// 2-byte unsigned integer. Default for <see langword="ushort"/>.
        /// </summary>
        UInt16,

        /// <summary>
        /// Write the 3 right-most bytes of the integer.
        /// </summary>
        Int24,

        /// <summary>
        /// 4-byte integer. Default for <see langword="int"/>.
        /// </summary>
        Int32,

        /// <summary>
        /// 4-byte unsigned integer. Default for <see langword="uint"/>.
        /// </summary>
        UInt32,

        /// <summary>
        /// 8-byte integer. Default for <see langword="long"/>.
        /// </summary>
        Int64,

        /// <summary>
        /// 8-byte unsigned integer. Default for <see langword="ulong"/>.
        /// </summary>
        UInt64,

        /// <summary>
        /// Dynamic-length integer, alike UTF8: 7 bits of data,
        /// highest bit set to 1 means "continues to the next byte".
        /// </summary>
        DynamicInt,

        /// <summary>
        /// 1 byte per character, no 0-termination. Default for <see langword="string"/>.
        /// Length must be given by <see cref="PiffStringLengthAttribute"/>.
        /// </summary>
        Ascii,

        /// <summary>
        /// 1 byte per character, 0-termination.
        /// </summary>
        AsciiZero,

        /// <summary>
        /// 1-4 bytes per character, 0-termination.
        /// </summary>
        Utf8Zero,

        /// <summary>
        /// 0-terminated UTF-8 or UTF-16 string.
        /// UTF-16 string must start with byte order mark 0xFEFF.
        /// </summary>
        Utf8Or16Zero,

        /// <summary>
        /// 2 bytes per character, no 0-termination.
        /// Length must be given by <see cref="PiffStringLengthAttribute"/>.
        /// </summary>
        Ucs2,

        /// <summary>
        /// A <see cref="System.Guid"/> as a byte array (16 bytes).
        /// </summary>
        GuidBytes,

        /// <summary>
        /// Treat the value as a box with length, ID, and collection of properties.
        /// </summary>
        Box,

        /// <summary>
        /// Treat the value as just a collection of properties.
        /// </summary>
        InlineObject,
    }
}
