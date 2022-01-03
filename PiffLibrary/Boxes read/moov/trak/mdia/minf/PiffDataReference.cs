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
        /// When the box is present, there should be at least one.
        /// </summary>
        public int Count { get; set; } = 1;


        /// <summary>
        /// The array or URLs.
        /// </summary>
        [PiffArraySize(nameof(Count))]
        public PiffDataEntryUrl[] Url { get; set; } = new[] { new PiffDataEntryUrl() };

        #endregion
    }
}