namespace PiffLibrary.Boxes
{
    [BoxName("tmin")]
    public sealed class PiffHintMinRelativeTimeBox : PiffBoxBase
    {
        /// <summary>
        /// Smallest relative transmission time [milliseconds].
        /// </summary>
        public int Time { get; set; }
    }
}
