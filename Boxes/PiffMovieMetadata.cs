namespace PiffLibrary
{
    [BoxName("moov")]
    internal class PiffMovieMetadata
    {
        #region Properties

        public PiffMovieHeader MovieHeader { get; }


        public PiffProtectionInfo ProtectionHeader { get; }


        public PiffTrack AudioTrack { get; }


        public PiffTrack VideoTrack { get; }


        public PiffMovieExtended Extended { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieMetadata(PiffManifest manifest)
        {
            MovieHeader = new PiffMovieHeader(manifest.Created, manifest.Duration, manifest.TimeScale);
            ProtectionHeader = new PiffProtectionInfo(manifest.ProtectionSystemId, manifest.ProtectionData);
            AudioTrack = PiffTrack.CreateAudio(
                manifest.Channels, manifest.BitsPerSample, manifest.SamplingRate,
                manifest.Created, manifest.Duration, manifest.TimeScale, manifest.KeyIdentifier);
            VideoTrack = PiffTrack.CreateVideo(
                manifest.Width, manifest.Height, manifest.Created,
                manifest.Duration, manifest.TimeScale, manifest.KeyIdentifier);
            Extended = new PiffMovieExtended(manifest.Duration, manifest.TimeScale);
        }

        #endregion
    }
}