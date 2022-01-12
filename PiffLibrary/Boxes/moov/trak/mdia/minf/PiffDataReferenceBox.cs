namespace PiffLibrary.Boxes
{
    [BoxName("dref")]
    [ChildType(typeof(PiffDataEntryUrlBox))]
    [ChildType(typeof(PiffDataEntryUrnBox))]
    public sealed class PiffDataReferenceBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of elements in <see cref="PiffBoxBase.Children"/>.
        /// When the box is present, there should be at least one.
        /// </summary>
        public int Count { get; set; }

        #endregion
    }
}