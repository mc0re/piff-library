namespace PiffLibrary
{
    [BoxName("edts")]
    internal class PiffEditList : PiffBoxBase
    {
        /// <summary>
        /// An array of edit items.
        /// </summary>
        public PiffEditListItem[] Items { get; set; }
    }
}
