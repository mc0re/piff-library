using System;


namespace PiffLibrary
{
    /// <summary>
    /// For box hierarchy, this property shall be placed after the derived ones.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class AfterDescendantsAttribute : Attribute
    {
    }
}
