using System;


namespace PiffLibrary.Boxes
{
    [BoxName("moov")]
    [ChildType(typeof(PiffMovieHeaderBox))]
    [ChildType(typeof(PiffExtensionBox))]
    [ChildType(typeof(PiffTrackBox))]
    [ChildType(typeof(PiffMovieExtendedBox))]
    public sealed class PiffMovieMetadataBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieMetadataBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// Creates a lot of default information.
        /// </summary>
        /// <param name="manifest">Movie data</param>
        public PiffMovieMetadataBox(PiffManifest manifest)
        {
            var maxDuration = Math.Max(manifest.Audio.Duration, manifest.Video.Duration);

            Childen = new PiffBoxBase[]
            {
                new PiffMovieHeaderBox(manifest.Created, maxDuration, manifest.TimeScale),
                PiffExtensionBox.ProtectionInfo(
                    manifest.ProtectionSystemId, manifest.ProtectionData),
                PiffTrackBox.CreateAudio(
                    manifest.AudioTrackId, manifest.Audio, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier),
                PiffTrackBox.CreateVideo(
                    manifest.VideoTrackId, manifest.Video, manifest.Created, manifest.TimeScale, manifest.KeyIdentifier),
                new PiffMovieExtendedBox(maxDuration, 2)
            };
        }

        #endregion
    }
}