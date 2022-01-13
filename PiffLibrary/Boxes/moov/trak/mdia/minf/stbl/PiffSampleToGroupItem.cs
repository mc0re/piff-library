namespace PiffLibrary.Boxes
{
    public sealed class PiffSampleToGroupItem
    {
        /// <summary>
        /// The number of consequtive samples with the same characteristics.
        /// </summary>
        public uint SampleCount { get; set; }


        /// <summary>
        /// Index into <see cref="PiffSampleGroupDescriptionBox"/> box. Starts with 1.
        /// 0 means the sample is not a member of a group of this type.
        /// </summary>
        public uint GroupDescriptionIndex { get; set; }
    }
}
