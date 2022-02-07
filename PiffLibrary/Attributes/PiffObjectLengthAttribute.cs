using System;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// The property, marke swith this attribute, defines the length of the object data.
    /// The length is in bytes and starts counting after the marked property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class PiffObjectLengthAttribute : Attribute
    {
    }
}