using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the number of elements in the array value.
    /// </summary>
    internal class PiffArraySizeAttribute : Attribute
    {
        public int Size { get; }


        public PiffArraySizeAttribute(int size)
        {
            Size = size;
        }
    }
}