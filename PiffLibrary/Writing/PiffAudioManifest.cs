namespace PiffLibrary
{
    public class PiffAudioManifest
    {
        /// <summary>
        /// In <see cref="PiffManifest.TimeScale"/> units.
        /// </summary>
        public ulong Duration { get; set; }

        public int BitRate { get; set; }

        public ushort Channels { get; set; }

        public ushort BitsPerSample { get; set; }

        public ushort SamplingRate { get; set; }

        /// <summary>
        /// 4CC.
        /// </summary>
        public string CodecId { get; set; }

        public byte[] CodecData { get; set; }
    }
}