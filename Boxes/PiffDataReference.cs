namespace PiffLibrary
{
    [BoxName("dref")]
    internal class PiffDataReference
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;

        /// <summary>
        /// The number of elements in <see cref="Url"/>.
        /// </summary>
        public int Count { get; } = 1;

        /// <summary>
        /// The array has <see cref="Count"/> elements.
        /// </summary>
        public PiffDataEntryUrl[] Url { get; } = new[] { new PiffDataEntryUrl() };

        #endregion
    }
}