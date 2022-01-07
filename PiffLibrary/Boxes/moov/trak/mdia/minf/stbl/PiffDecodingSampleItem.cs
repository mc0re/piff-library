namespace PiffLibrary
{
    internal class PiffDecodingSampleItem
    {
        /// <summary>
        /// The number of consecutive samples that have the given duration.
        /// </summary>
        public uint Count { get; set; }


        /// <summary>
        /// Delta of these samples from the previous entry
        /// (for the first entry - from the first item in <see cref="PiffEditList"/>)
        /// in time scale of the media.
        /// </summary>
        public uint Delta { get; set; }
    }
}