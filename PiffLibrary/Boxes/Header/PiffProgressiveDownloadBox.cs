namespace PiffLibrary.Boxes
{
    [BoxName("pdin")]
    public sealed class PiffProgressiveDownloadBox : PiffFullBoxBase
    {
        /// <summary>
        /// A set of points for inter- or extrapolation.
        /// </summary>
        public PiffProgressiveDownloadItem[] RateMap { get; set; }
    }
}