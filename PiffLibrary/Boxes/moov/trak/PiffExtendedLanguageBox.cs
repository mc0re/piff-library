namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Language information based on RFC 4646.
    /// </summary>
    [BoxName("elng")]
    public sealed class PiffExtendedLanguageBox : PiffFullBoxBase
    {
        /// <summary>
        /// Language variant definition, e.g. "en-US".
        /// </summary>
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string Language { get; set; }
    }
}
