namespace PiffLibrary
{
    public class PiffVideoManifest
    {
        /// <summary>
        /// In <see cref="PiffManifest.TimeScale"/> units.
        /// </summary>
        public ulong Duration { get; set; }

        public int BitRate { get; set; }

        public ushort Width { get; set; }

        public ushort Height { get; set; }

        /// <summary>
        /// 4CC.
        /// </summary>
        public string CodecId { get; set; }

        public byte[] CodecData { get; set; }
    }
}