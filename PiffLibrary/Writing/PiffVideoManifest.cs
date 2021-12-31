namespace PiffLibrary
{
    public class PiffVideoManifest
    {
        /// <summary>
        /// In <see cref="PiffManifest.TimeScale"/> units.
        /// </summary>
        public long Duration { get; set; }

        public int BitRate { get; set; }

        public short Width { get; set; }

        public short Height { get; set; }

        /// <summary>
        /// 4CC.
        /// </summary>
        public string CodecId { get; set; }

        public byte[] CodecData { get; set; }
    }
}