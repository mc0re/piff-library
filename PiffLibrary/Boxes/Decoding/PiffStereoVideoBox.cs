namespace PiffLibrary.Boxes
{
    [BoxName("stvi")]
    public sealed class PiffStereoVideoBox : PiffFullBoxBase
    {
        /// <summary>
        /// First 30 bits are reserved (set to 0).
        /// 
        /// Of the other 2 bits:
        /// - Bit 0 set - it is allowed to display the right view on a mono display
        /// - Bit 1 set - it is allowed to display the left view on a mono display
        /// </summary>
        public uint SingleViewAllowed { get; set; }


        /// <summary>
        /// 1 - frame packing as in ISO/IEC 14496-10
        /// 2 - arrangement type as in Annex L of ISO/ICE 13818-2
        /// 3 - stereo scheme as in ISO/IEC 23000-11
        /// </summary>
        public uint StereoScheme { get; set; }


        public uint Length { get; set; }


        /// <summary>
        /// Depends on <see cref="StereoScheme"/>.
        /// </summary>
        [PiffArraySize(nameof(Length))]
        public byte[] StereoIndicationType { get; set; }
    }
}