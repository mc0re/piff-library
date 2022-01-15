namespace PiffLibrary.Boxes
{
    public class PiffItemInfoEntryExtension
    {
        /// <summary>
        /// Identifies the extension fields.
        /// Only "fdel" is defined in the standard.
        /// </summary>
        [PiffStringLength(4)]
        public string ExtensionType { get; set; }


        /// <summary>
        /// See RFC 2616.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string ContentLocation { get; set; }


        /// <summary>
        /// MD5 digest of the file.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string ContentMd5 { get; set; }


        /// <summary>
        /// Total length in bytes of the un-encoded file.
        /// </summary>
        public ulong ContentLength { get; set; }


        /// <summary>
        /// Total length in bytes of the encoded file.
        /// </summary>
        public ulong TransferLength { get; set; }


        public byte GroupCount { get; set; }


        /// <summary>
        /// A file group to which the file item belongs. See 3GPP TS 26.346.
        /// </summary>
        [PiffArraySize(nameof(GroupCount))]
        public uint[] GroupIds { get; set; }
    }
}
