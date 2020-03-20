using System;


namespace PiffLibrary
{
    public class PiffManifest
    {
        public DateTime Created { get; internal set; }

        public TimeSpan TotalDuration { get; internal set; }

        public int TimeScale { get; internal set; }

        public PiffAudioManifest Audio { get; set; }

        public PiffVideoManifest Video { get; set; }

        public Guid KeyIdentifier { get; internal set; }

        public Guid ProtectionSystemId { get; internal set; }

        public byte[] ProtectionData { get; internal set; }
    }
}