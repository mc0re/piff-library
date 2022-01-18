using System;


namespace PiffLibrary
{
    /// <summary>
    /// For box hierarchy, this property shall be placed after the derived ones.
    /// If not specified, the inherited properties are placed before the derived ones
    /// in the order of hierarchy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class AfterDescendantsAttribute : Attribute
    {
    }
}
