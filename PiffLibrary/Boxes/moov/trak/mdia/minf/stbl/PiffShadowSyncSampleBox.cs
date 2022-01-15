namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Relation between non-sync and sync samples, used for seeking.
    /// </summary>
    [BoxName("stsh")]
    public sealed class PiffShadowSyncSampleBox : PiffFullBoxBase
    {
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffShadowSyncItem[] Shadows { get; set; }
    }
}