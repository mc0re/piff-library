namespace PiffLibrary.Boxes
{
    [BoxName("tmax")]
    public sealed class PiffHintMaxRelativeTimeBox : PiffBoxBase
    {
        /// <summary>
        /// Largest relative transmission time [millisecond].
        /// </summary>
        public int Time { get; set; }
    }
}
