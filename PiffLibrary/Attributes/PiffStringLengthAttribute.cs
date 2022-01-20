using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the size of an ASCII string for string value,
    /// or the size of string elements for a string array value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class PiffStringLengthAttribute : Attribute
    {
        /// <summary>
        /// Property name to get the length.
        /// </summary>
        public string LengthProperty { get; }


        /// <summary>
        /// The number of bytes in the string.
        /// </summary>
        public int Length { get; }


        public PiffStringLengthAttribute(int length)
        {
            Length = length;
        }


        /// <summary>
        /// The number of bytes is defined by a property with the given name.
        /// The type of property must be convertible to integer.
        /// </summary>
        /// <param name="sizeProperty">
        /// Property name (use nameof). Can be a public or a private property.
        /// </param>
        public PiffStringLengthAttribute(string lengthProperty)
        {
            LengthProperty = lengthProperty;
        }
    }
}