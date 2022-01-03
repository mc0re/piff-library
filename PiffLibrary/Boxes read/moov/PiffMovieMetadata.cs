using System;

namespace PiffLibrary
{
    [BoxName("moov")]
    internal class PiffMovieMetadata : PiffBoxBase
    {
        #region Properties

        public PiffMovieHeader MovieHeader { get; set; }


        public PiffExtensionBox ProtectionHeader { get; set; }


        public PiffTrack AudioTrack { get; set; }


        public PiffTrack VideoTrack { get; set; }


        public PiffMovieExtended Extended { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieMetadata()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        /// <param name="manifest">Movie data</param>
        public PiffMovieMetadata(PiffManifest manifest)
        {
            var maxDuration = Math.Max(manifest.Audio.Duration, manifest.Video.Duration);
            MovieHeader = new PiffMovieHeader(manifest.Created, maxDuration, manifest.TimeScale);

            ProtectionHeader = PiffExtensionBox.ProtectionInfo(
                manifest.ProtectionSystemId, manifest.ProtectionData);

            AudioTrack = PiffTrack.CreateAudio(
                manifest.AudioTrackId, manifest.Audio, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier);

            VideoTrack = PiffTrack.CreateVideo(
                manifest.VideoTrackId, manifest.Video, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier);

            Extended = new PiffMovieExtended(maxDuration, 2);
        }

        #endregion
    }
}