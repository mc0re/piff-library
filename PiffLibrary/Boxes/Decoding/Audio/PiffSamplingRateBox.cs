namespace PiffLibrary.Boxes
{
    [BoxName("srat")]
    public sealed class PiffSamplingRateBox : PiffFullBoxBase
    {
        /// <summary>
        /// Actual sampling rate.
        /// </summary>
        public uint SamplingRate { get; set; }
    }
}