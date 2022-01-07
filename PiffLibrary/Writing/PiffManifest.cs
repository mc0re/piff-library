using System;


namespace PiffLibrary
{
    public class PiffManifest
    {
        public DateTime Created { get; set; }

        public TimeSpan TotalDuration { get; set; }

        /// <summary>
        /// How many units are in 1 second.
        /// </summary>
        public uint TimeScale { get; set; }

        public PiffAudioManifest Audio { get; set; }

        public uint AudioTrackId { get; set; }

        public PiffVideoManifest Video { get; set; }

        public uint VideoTrackId { get; set; }

        public Guid KeyIdentifier { get; set; }

        public Guid ProtectionSystemId { get; set; }

        public byte[] ProtectionData { get; set; }
    }
}