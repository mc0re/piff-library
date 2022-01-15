namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Direct storage of XML.
    /// </summary>
    [BoxName("xml ")]
    public sealed class PiffXmlBox : PiffFullBoxBase
    {
        [PiffDataFormat(PiffDataFormats.Utf8Or16Zero)]
        public string Xml { get; set; }
    }
}
