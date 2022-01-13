namespace PiffLibrary.Boxes
{
    public sealed class PiffShadowSyncItem
    {
        /// <summary>
        /// Which sample number is been shadowed.
        /// </summary>
        public uint ShadowedSample { get; set; }


        /// <summary>
        /// Sample number replacing the shadowed one.
        /// </summary>
        public uint SyncSample { get; set; }
    }
}