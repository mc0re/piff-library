namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Data for the items where <see cref="PiffItemLocationResource.ConstructionMethod"/>
    /// indicates the item's data extents are stored within this box.
    /// </summary>
    [BoxName("idat")]
    public sealed class PiffItemDataBox : PiffBoxBase
    {
        public byte[] Data { get; set; }
    }
}
