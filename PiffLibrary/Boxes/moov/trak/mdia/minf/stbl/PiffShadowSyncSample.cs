namespace PiffLibrary
{
    /// <summary>
    /// Relation between non-sync and sync samples, used for seeking.
    /// </summary>
    [BoxName("stsh")]
    internal class PiffShadowSyncSample
    {
        public uint Count { get; set; }


        [PiffArraySize(nameof(Count))]
        public PiffShadowSyncItem[] Shadows { get; set; }
    }
}