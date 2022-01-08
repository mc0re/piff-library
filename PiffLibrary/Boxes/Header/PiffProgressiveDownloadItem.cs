namespace PiffLibrary
{
    internal class PiffProgressiveDownloadItem
    {
        /// <summary>
        /// Download rate [bytes/second].
        /// </summary>
        public uint Rate { get; set; }


        /// <summary>
        /// Suggested delay before playing the file [milliseconds].
        /// </summary>
        public uint Delay { get; set; }
    }
}