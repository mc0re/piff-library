namespace PiffLibrary
{
    public class PiffAudioManifest
    {
        /// <summary>
        /// In <see cref="PiffManifest.TimeScale"/> units.
        /// </summary>
        public long Duration { get; set; }

        public int BitRate { get; set; }

        public short Channels { get; set; }

        public short BitsPerSample { get; set; }

        public ushort SamplingRate { get; set; }

        /// <summary>
        /// 4CC.
        /// </summary>
        public string CodecId { get; set; }

        public byte[] CodecData { get; set; }
    }
}