namespace PiffLibrary.Boxes
{
    [BoxName("tims")]
    public sealed class PiffTimeScaleEntryBox : PiffBoxBase
    {
        public uint TimeScale { get; set; }
    }
}
