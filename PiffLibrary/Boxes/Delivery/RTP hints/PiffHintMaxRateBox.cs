namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Maximum data rate.
    /// </summary>
    [BoxName("maxr")]
    public sealed class PiffHintMaxRateBox : PiffBoxBase
    {
        /// <summary>
        /// Measure period [milliseconds].
        /// </summary>
        public uint Period { get; set; }


        /// <summary>
        /// Max bytes (including RTP headers) sent in any <see cref="Period"/> milliseconds.
        /// </summary>
        public uint Bytes { get; set; }
    }
}
