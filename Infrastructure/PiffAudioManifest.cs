namespace PiffLibrary
{
    public class PiffAudioManifest
    {
        /// <summary>
        /// In <see cref="PiffManifest.TimeScale"/> units.
        /// </summary>
        public long Duration { get; internal set; }

        public int BitRate { get; private set; }

        public short Channels { get; internal set; }

        public short BitsPerSample { get; internal set; }

        public ushort SamplingRate { get; internal set; }

        /// <summary>
        /// 4CC.
        /// </summary>
        public string CodecId { get; private set; }

        public byte[] CodecData { get; private set; }
    }
}