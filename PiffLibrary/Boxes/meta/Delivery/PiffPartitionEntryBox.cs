namespace PiffLibrary.Boxes
{
    [BoxName("paen")]
    [ChildType(typeof(PiffFilePartitionBox))]
    [ChildType(typeof(PiffFecReservoirBox))]
    [ChildType(typeof(PiffFileReservoirBox))]
    public sealed class PiffPartitionEntryBox : PiffBoxBase
    {
    }
}
