using System;


namespace PiffLibrary
{
    /// <summary>
    /// Specifies the type of a child box, if the marked box is a container.
    /// Multiple types are defined by multiple attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class ChildTypeAttribute : Attribute
    {
        public Type ChildType { get; }


        public ChildTypeAttribute(Type childType) => ChildType = childType;
    }
}