using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the number of elements in the array value.
    /// </summary>
    internal class PiffArraySizeAttribute : Attribute
    {
        public string SizeProp { get; }
        
        public int Size { get; }


        /// <summary>
        /// Fixed array size.
        /// </summary>
        public PiffArraySizeAttribute(int size)
        {
            Size = size;
        }


        /// <summary>
        /// The number of elements is defined by the given property.
        /// </summary>
        public PiffArraySizeAttribute(string sizeProperty)
        {
            SizeProp = sizeProperty;
        }
    }
}