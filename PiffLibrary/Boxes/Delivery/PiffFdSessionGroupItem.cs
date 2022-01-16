namespace PiffLibrary.Boxes
{
    public sealed class PiffFdSessionGroupItem
    {
        public byte EntryCount { get; set; }


        /// <summary>
        /// A file group that the session group complies with.
        /// </summary>
        [PiffArraySize(nameof(EntryCount))]
        public uint[] GroupId { get; set; }


        public ushort GroupChannelCount { get; set; }


        /// <summary>
        /// Track IDs of the FD hint track for the session group.
        /// </summary>
        [PiffArraySize(nameof(GroupChannelCount))]
        public uint[] HintTrackIds { get; set; }
    }
}
