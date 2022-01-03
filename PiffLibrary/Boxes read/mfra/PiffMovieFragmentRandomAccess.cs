using System.Collections.Generic;


namespace PiffLibrary
{
    [BoxName("mfra")]
    internal class PiffMovieFragmentRandomAccess : PiffBoxBase
    {
        #region Properties

        public PiffTrackFragmentRandomAccess Audio { get; set; }

        public PiffTrackFragmentRandomAccess Video { get; set; }

        public PiffMovieFragmentRandomAccessOffset Length { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieFragmentRandomAccess()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
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
