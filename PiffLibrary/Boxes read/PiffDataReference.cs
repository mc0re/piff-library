namespace PiffLibrary
{
    [BoxName("dref")]
    internal class PiffDataReference : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        /// <summary>
        /// The number of elements in <see cref="Url"/>.
        /// </summary>
        public int Count { get; set; } = 1;


        /// <summary>
        /// The array has <see cref="Count"/> elements.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        public PiffDataEntryUrl[] Url { get; set; } = new[] { new PiffDataEntryUrl() };

        #endregion
    }
}