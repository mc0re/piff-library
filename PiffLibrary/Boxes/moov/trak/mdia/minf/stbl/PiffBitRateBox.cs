namespace PiffLibrary.Boxes
{
    [BoxName("btrt")]
    public sealed class PiffBitRateBox : PiffBoxBase
    {
        /// <summary>
        /// Size of the decoding buffer [bytes].
        /// </summary>
        public uint BufferSize { get; set; }


        /// <summary>
        /// Maximum rate in bits per second.
        /// </summary>
        public uint MaxBitrate { get; set; }


        /// <summary>
        /// Average over the entire presentation rate in bits per second.
        /// </summary>
        public uint AverageBitrate { get; set; }
    }
}