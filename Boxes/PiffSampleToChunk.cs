namespace PiffLibrary
{
    [BoxName("stsc")]
    internal class PiffSampleToChunk
    {
        #region Properties

        public byte Version { get; } = 0;

        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;

        /// <summary>
        /// The number of elements in the following array.
        /// </summary>
        public int Count { get; } = 0;

        #endregion
    }
}