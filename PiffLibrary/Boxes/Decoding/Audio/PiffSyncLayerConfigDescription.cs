namespace PiffLibrary.Boxes
{
    public sealed class PiffSyncLayerConfigDescription
    {
        #region Properties

        /// <summary>
        /// Defines a set of synchronization flags.
        /// 0 - Define the flags explicitly
        /// 1 - Null
        /// 2 - MP4
        /// </summary>
        public byte PredefinedSync { get; set; } = 2;

        #endregion
    }
}