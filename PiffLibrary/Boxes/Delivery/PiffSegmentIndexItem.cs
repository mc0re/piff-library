namespace PiffLibrary.Boxes
{
    public sealed class PiffSegmentIndexItem
    {
        /// <summary>
        /// 1 bit - reference type
        ///   - 0 - media, the reference is to media content
        ///   - 1 - index, the reference is to a <see cref="PiffSegmentIndexBox"/>
        /// 31 bits - size of the referenced item, in bytes
        /// </summary>
        public uint TypeAndSize { get; set; }


        /// <summary>
        /// If <see cref="TypeAndSize"/> type is 1, this is
        /// the sum of all <see cref="SubsegmentDuration"/> fields
        /// in the referenced <see cref="PiffSegmentIndexBox"/>.
        /// 
        /// If it is 0, it is the duration of the data
        /// in <see cref="PiffSegmentIndexBox.TimeScale"/> units.
        /// </summary>
        public uint SubsegmentDuration { get; set; }


        /// <summary>
        /// 1 bit - starts with SAP (stream access point)
        ///   If 1, the referenced sub-segments start with a SAP
        /// 3 bits - SAP type
        ///   0 or a type 1-6
        /// 28 bits - SAP delta time of the first SAP (if defined by the type).
        /// </summary>
        public uint Sap { get; set; }
    }
}
