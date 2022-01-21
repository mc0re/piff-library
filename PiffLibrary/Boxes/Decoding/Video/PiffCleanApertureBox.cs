namespace PiffLibrary.Boxes
{
    [BoxName("clap")]
    public sealed class PiffCleanApertureBox : PiffBoxBase
    {
        /// <summary>
        /// LeftMostX = (<see cref="CleanApertureWidth"/> - 1) / 2
        /// </summary>
        public PiffFractionItem CleanApertureWidth { get; set; }


        /// <summary>
        /// TopMostY = (<see cref="CleanApertureHeight"/> - 1) / 2
        /// </summary>
        public PiffFractionItem CleanApertureHeight { get; set; }


        /// <summary>
        /// Horizontal offset of the image center; usually 0.
        /// 
        /// CenterX = <see cref="HorizontalOffset"/> + (<see cref="PiffVideoSampleEntryBox.Width"/> - 1) / 2
        /// </summary>
        public PiffFractionItem HorizontalOffset { get; set; }


        /// <summary>
        /// Vertical offset of the image center; usually 0.
        /// 
        /// CenterY = <see cref="VerticalOffset"/> + (<see cref="PiffVideoSampleEntryBox.Height"/> - 1) / 2
        /// </summary>
        public PiffFractionItem VerticalOffset { get; set; }
    }
}