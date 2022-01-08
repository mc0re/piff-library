namespace PiffLibrary
{
    [BoxName("dref")]
    [ChildType(typeof(PiffDataEntryUrl))]
    [ChildType(typeof(PiffDataEntryUrn))]
    internal class PiffDataReference : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of child elements.
        /// When the box is present, there should be at least one.
        /// </summary>
        public int Count { get; set; }

        #endregion
    }
}