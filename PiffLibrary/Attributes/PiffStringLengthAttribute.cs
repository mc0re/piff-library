using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the size of an ASCII string for string value,
    /// or the size of string elements for a string array value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class PiffStringLengthAttribute : Attribute
    {
        /// <summary>
        /// The number of bytes in the string.
        /// </summary>
        public int Length { get; }


        public PiffStringLengthAttribute(int length)
        {
            Length = length;
        }
    }
}