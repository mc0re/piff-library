namespace PiffLibrary.Boxes
{
    public abstract class PiffFullBoxBase : PiffBoxBase
    {
        /// <summary>
        /// Box version. Determines the presence or
        /// the number of bits of some properties.
        /// </summary>
        public byte Version { get; set; }


        /// <summary>
        /// Bit-flags. Usually determine the presence of some properties.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; }
    }
}