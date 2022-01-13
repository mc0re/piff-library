using System;


namespace PiffLibrary
{
    /// <summary>
    /// Defines the number of elements in the array value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class PiffArraySizeAttribute : Attribute
    {
        public string SizeProp { get; }
        
        public int Size { get; }


        /// <summary>
        /// Fixed array size.
        /// </summary>
        /// <remarks>
        /// If the array size is 0, the array itself may be <see langword="null"/>.
        /// </remarks>
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
        /// <remarks>
        /// If the array size is 0, the array itself may be <see langword="null"/>.
        /// </remarks>
        public PiffArraySizeAttribute(string sizeProperty)
        {
            SizeProp = sizeProperty;
        }
    }
}