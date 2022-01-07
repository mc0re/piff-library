using System;


namespace PiffLibrary
{
    /// <summary>
    /// For box hierarchy, this property shall be placed before the derived ones.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class BeforeDescendantsAttribute : Attribute
    {
    }
}
