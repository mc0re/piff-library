namespace PiffLibrary.Boxes
{
    public sealed class PiffSubsegmentIndexRange
    {
        /// <summary>
        /// Level to which this subsegment is assigned.
        /// </summary>
        public byte Level { get; set; }


        /// <summary>
        /// Size of this subsegment.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Size { get; set; }
    }
}
