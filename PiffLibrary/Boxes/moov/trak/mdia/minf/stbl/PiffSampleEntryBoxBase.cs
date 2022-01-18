namespace PiffLibrary.Boxes
{
    [ChildType(typeof(PiffBitRateBox))]
    public abstract class PiffSampleEntryBoxBase : PiffBoxBase
    {
        [PiffArraySize(6)]
        public byte[] Reserved1 { get; } = { 0, 0, 0, 0, 0, 0 };


        /// <summary>
        /// Index to the data reference.
        /// </summary>
        public short DataReferenceIndex { get; set; }
    }
}