using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("pdin")]
    internal class PiffProgressiveDownload : PiffFullBoxBase
    {
        /// <summary>
        /// A set of points for inter- or extrapolation.
        /// </summary>
        public PiffProgressiveDownloadItem[] RateMap { get; set; }
    }
}