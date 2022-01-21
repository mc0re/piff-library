namespace PiffLibrary.Boxes
{
    [BoxName("pasp")]
    public sealed class PiffPixelAspectRatioBox : PiffBoxBase
    {
        /// <summary>
        /// Horizontal ratio part.
        /// </summary>
        public uint HSpacing { get; set; }


        /// <summary>
        /// Vertical ratio part.
        /// </summary>
        public uint VSpacing { get; set; }
    }
}