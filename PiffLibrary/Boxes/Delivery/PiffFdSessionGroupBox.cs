namespace PiffLibrary.Boxes
{
    /// <summary>
    /// A list of sessions, all file groups and hint tracks.
    /// Only one session group should be processed at any time.
    /// </summary>
    [BoxName("segr")]
    public sealed class PiffFdSessionGroupBox : PiffBoxBase
    {
        public ushort SessionGroupCount { get; set; }

        [PiffArraySize(nameof(SessionGroupCount))]
        public PiffFdSessionGroupItem[] Items { get; set; }
    }
}
