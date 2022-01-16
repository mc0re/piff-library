namespace PiffLibrary.Boxes
{
    public sealed class PiffGroupIdToNameItem
    {
        public uint GroupId { get; set; }


        /// <summary>
        /// File group name.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Utf8Zero)]
        public string GroupName { get; set; }
    }
}
