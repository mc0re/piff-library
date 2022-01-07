using System;

namespace PiffLibrary
{
    [BoxName("moov")]
    [ChildType(typeof(PiffMovieHeader))]
    [ChildType(typeof(PiffExtensionBox))]
    [ChildType(typeof(PiffTrack))]
    [ChildType(typeof(PiffMovieExtended))]
    internal class PiffMovieMetadata : PiffBoxBase
    {
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

            Childen = new PiffBoxBase[]
            {
                new PiffMovieHeader(manifest.Created, maxDuration, manifest.TimeScale),
                PiffExtensionBox.ProtectionInfo(
                    manifest.ProtectionSystemId, manifest.ProtectionData),
                PiffTrack.CreateAudio(
                    manifest.AudioTrackId, manifest.Audio, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier),
                PiffTrack.CreateVideo(
                    manifest.VideoTrackId, manifest.Video, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier),
                new PiffMovieExtended(maxDuration, 2)
            };
        }

        #endregion
    }
}