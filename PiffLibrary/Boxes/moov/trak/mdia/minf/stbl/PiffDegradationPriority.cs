namespace PiffLibrary
{
    [BoxName("stdp")]
    internal class PiffDegradationPriority : PiffFullBoxBase
    {
        /// <summary>
        /// The number of items is <see cref="PiffSampleSize.SampleCount"/>.
        /// The meaning is not really defined in ISO spec.
        /// </summary>
        public ushort[] Priority { get; set; }
    }
}