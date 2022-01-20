namespace PiffLibrary.Boxes
{
    [BoxName("tsro")]
    public sealed class PiffTimeOffsetBox : PiffBoxBase
    {
        /// <summary>
        /// Offset to the play time, can be 0.
        /// </summary>
        public int Offset { get; set; }
    }
}
