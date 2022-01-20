namespace PiffLibrary.Boxes
{
    [BoxName("snro")]
    public sealed class PiffSequenceOffsetBox : PiffBoxBase
    {
        /// <summary>
        /// Offset to sequence number, can be 0.
        /// </summary>
        public int Offset { get; set; }
    }
}
