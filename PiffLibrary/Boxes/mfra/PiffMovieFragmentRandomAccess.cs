using System.Collections.Generic;


namespace PiffLibrary
{
    [BoxName("mfra")]
    [ChildType(typeof(PiffTrackFragmentRandomAccess))]
    [ChildType(typeof(PiffMovieFragmentRandomAccessOffset))]
    internal class PiffMovieFragmentRandomAccess : PiffBoxBase
    {
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
            uint audioTrackId, IEnumerable<PiffSampleOffset> audio,
            uint videoTrackId, IEnumerable<PiffSampleOffset> video)
        {
            var audioBox = new PiffTrackFragmentRandomAccess(audioTrackId, audio);
            var videoBox = new PiffTrackFragmentRandomAccess(videoTrackId, video);

            var len = HeaderLength + audioBox.GetLength() + videoBox.GetLength() + 16;

            Childen = new PiffBoxBase[]
            {
                audioBox, videoBox, new PiffMovieFragmentRandomAccessOffset(len)
            };
        }

        #endregion
    }
}
