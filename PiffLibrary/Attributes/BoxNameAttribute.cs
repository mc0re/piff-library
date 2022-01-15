using System;


namespace PiffLibrary
{
    /// <summary>
    /// Specifies the name of the box containing a given object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class BoxNameAttribute : Attribute
    {
        public string Name { get; }


        public BoxNameAttribute(string name) => Name = name;
    }
}
