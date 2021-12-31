using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the size of a fixed string for string value,
    /// or the size of string elements for a string array value.
    /// </summary>
    internal class PiffDataLengthAttribute : Attribute
    {
        public int Length { get; }


        public PiffDataLengthAttribute(int length)
        {
            Length = length;
        }
    }
}