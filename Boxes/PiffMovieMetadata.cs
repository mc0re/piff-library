using System;

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
            var maxDuration = Math.Max(manifest.Audio.Duration, manifest.Video.Duration);
            MovieHeader = new PiffMovieHeader(manifest.Created, maxDuration, manifest.TimeScale);

            ProtectionHeader = new PiffProtectionInfo(
                manifest.ProtectionSystemId, manifest.ProtectionData);

            AudioTrack = PiffTrack.CreateAudio(
                1, manifest.Audio, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier);

            VideoTrack = PiffTrack.CreateVideo(
                2, manifest.Video, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier);

            Extended = new PiffMovieExtended(maxDuration);
        }

        #endregion
    }
}