namespace PiffLibrary.Boxes
{
    [BoxName("stdp")]
    public sealed class PiffDegradationPriorityBox : PiffFullBoxBase
    {
        /// <summary>
        /// The number of items is <see cref="PiffSampleSize.SampleCount"/>.
        /// The meaning is not really defined in ISO spec.
        /// </summary>
        public ushort[] Priority { get; set; }
    }
}