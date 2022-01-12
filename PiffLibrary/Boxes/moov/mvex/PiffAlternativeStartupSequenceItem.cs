namespace PiffLibrary.Boxes
{
    public sealed class PiffAlternativeStartupSequenceItem
    {
        /// <summary>
        /// Parameter to match in "sgbp" box.
        /// </summary>
        public uint GroupingTypeParameter { get; set; }


        /// <summary>
        /// No value of sample offset in the corresponding <see cref="PiffSampleToGroupBox"/>
        /// shall be smaller than this value.
        /// </summary>
        public int MinStartupOffset { get; set; }
    }
}