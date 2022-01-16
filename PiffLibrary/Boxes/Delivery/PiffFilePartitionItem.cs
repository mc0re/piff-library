namespace PiffLibrary.Boxes
{
    public sealed class PiffFilePartitionItem
    {
        /// <summary>
        /// The number of consecutive source blocks of size <see cref="BlockSize"/>.
        /// </summary>
        public ushort BlockCount { get; set; }


        /// <summary>
        /// THe size of a block [bytes].
        /// Should be a multiple of <see cref="PiffFilePartitionBox.EncodingSymbolLength"/>.
        /// </summary>
        public uint BlockSize { get; set; }
    }
}
