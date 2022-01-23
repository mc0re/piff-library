using System;


namespace PiffLibrary.Boxes
{
    [BoxName("moov")]
    [ChildType(typeof(PiffMovieHeaderBox))]
    [ChildType(typeof(PiffExtensionBox))]
    [ChildType(typeof(PiffTrackBox))]
    [ChildType(typeof(PiffMovieExtendedBox))]
    [ChildType(typeof(PiffUserDataBox))]
    [ChildType(typeof(PiffMetadataBox))]
    [ChildType(typeof(PiffMetadataContainerBox))]
    [ChildType(typeof(PiffProtectionSystemSpecificHeaderBox))] // PlayReady addition
    public sealed class PiffMovieBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// Creates a lot of default information.
        /// </summary>
        /// <param name="manifest">Movie data</param>
        public PiffMovieBox(PiffManifest manifest)
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