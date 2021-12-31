using System.Collections.Generic;


namespace PiffLibrary
{
    [BoxName("mfra")]
    internal class PiffMovieFragmentRandomAccess : PiffBoxBase
    {
        #region Properties

        public PiffTrackFragmentRandomAccess Audio { get; }

        public PiffTrackFragmentRandomAccess Video { get; }

        public PiffMovieFragmentRandomAccessOffset Length { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieFragmentRandomAccess(
            int audioTrackId, IEnumerable<PiffSampleOffset> audio,
            int videoTrackId, IEnumerable<PiffSampleOffset> video)
        {
            Audio = new PiffTrackFragmentRandomAccess(audioTrackId, audio);
            Video = new PiffTrackFragmentRandomAccess(videoTrackId, video);

            var len = 8 + Audio.GetLength() + Video.GetLength() + 16;
            Length = new PiffMovieFragmentRandomAccessOffset(len);
        }

        #endregion
    }
}
