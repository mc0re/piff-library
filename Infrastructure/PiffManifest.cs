using System;


namespace PiffLibrary
{
    public class PiffManifest
    {
        public DateTime Created { get; internal set; }

        public TimeSpan Duration { get; internal set; }

        public int TimeScale { get; internal set; }

        public short Channels { get; internal set; }

        public short BitsPerSample { get; internal set; }

        public ushort SamplingRate { get; internal set; }

        public short Width { get; internal set; }

        public short Height { get; internal set; }

        public Guid KeyIdentifier { get; internal set; }

        public Guid ProtectionSystemId { get; internal set; }

        public byte[] ProtectionData { get; internal set; }
    }
}