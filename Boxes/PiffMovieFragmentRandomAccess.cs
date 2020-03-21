using System.Collections.Generic;


namespace PiffLibrary
{
    [BoxName("mfra")]
    internal class PiffMovieFragmentRandomAccess
    {
        #region Properties

        public PiffTrackFragmentRandomAccess Audio { get; }

        public PiffTrackFragmentRandomAccess Video { get; }

        public PiffMovieFragmentRandomAccessOffset Length { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieFragmentRandomAccess(IEnumerable<PiffSampleOffsetV1> audio, IEnumerable<PiffSampleOffsetV1> video)
        {
            Audio = new PiffTrackFragmentRandomAccess(1, audio);
            Video = new PiffTrackFragmentRandomAccess(2, video);

            var len = 8 + Audio.GetLength() + Video.GetLength() + 16;
            Length = new PiffMovieFragmentRandomAccessOffset(len);
        }

        #endregion
    }
}
