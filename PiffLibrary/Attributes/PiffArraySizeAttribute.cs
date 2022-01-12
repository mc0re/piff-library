using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the number of elements in the array value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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
        /// The number of elements is defined by a property with the given name.
        /// The type of property must be convertible to integer.
        /// </summary>
        /// <param name="sizeProperty">
        /// Property name (use nameof). Can be a public or a private property.
        /// </param>
        public PiffArraySizeAttribute(string sizeProperty)
        {
            SizeProp = sizeProperty;
        }
    }
}