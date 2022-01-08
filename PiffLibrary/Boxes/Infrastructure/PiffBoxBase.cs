using System.Reflection;


namespace PiffLibrary
{
    /// <summary>
    /// Base class for all boxes with some common functionality.
    /// </summary>
    internal abstract class PiffBoxBase
    {
        /// <summary>
        /// Children boxes.
        /// All boxes have their children as the only fields,
        /// or as the last fields.
        /// </summary>
        [AfterDescendants]
        public PiffBoxBase[] Childen { get; set; } = new PiffBoxBase[0];


        public override string ToString() => GetType().GetCustomAttribute<BoxNameAttribute>().Name;
    }
}
