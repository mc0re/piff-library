﻿namespace PiffLibrary
{
    /// <summary>
    /// In which format shall the data be written to the file.
    /// </summary>
    public enum PiffDataFormats
    {
        /// <summary>
        /// Do not write this value.
        /// </summary>
        Skip,

        /// <summary>
        /// Treat the value as a collection of properties.
        /// </summary>
        Object,

        /// <summary>
        /// 1 byte per character, no 0-termination. Default for <see langword="string"/>.
        /// </summary>
        Ascii,

        /// <summary>
        /// 1-4 bytes per character, 0-termination.
        /// </summary>
        Utf8Zero,

        /// <summary>
        /// 2 bytes per character, no 0-termination.
        /// </summary>
        Ucs2,

        /// <summary>
        /// 1-byte integer. Default for <see langword="byte"/>.
        /// </summary>
        Int8,

        /// <summary>
        /// 2-byte integer. Default for <see langword="short"/>.
        /// </summary>
        Int16,

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
        /// A <see cref="System.Guid"/> as a byte array (16 bytes).
        /// </summary>
        GuidBytes,
    }
}