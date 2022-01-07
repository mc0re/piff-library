using System;


namespace PiffLibrary
{
    /// <summary>
    /// Specifies the type of a child box, if the marked box is a container.
    /// Multiple types are defined by multiple attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class ChildTypeAttribute : Attribute
    {
        public Type Child { get; }


        public ChildTypeAttribute(Type child) => Child = child;
    }
}