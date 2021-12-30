namespace PiffLibrary
{
    public class PiffVideoManifest
    {
        /// <summary>
        /// In <see cref="PiffManifest.TimeScale"/> units.
        /// </summary>
        public long Duration { get; internal set; }

        public int BitRate { get; private set; }

        public short Width { get; private set; }

        public short Height { get; private set; }

        /// <summary>
        /// 4CC.
        /// </summary>
        public string CodecId { get; private set; }

        public byte[] CodecData { get; private set; }
    }
}