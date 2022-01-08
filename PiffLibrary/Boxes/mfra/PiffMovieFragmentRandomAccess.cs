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
            var offsetBox = new PiffMovieFragmentRandomAccessOffset();

            Childen = new PiffBoxBase[]
            {
                audioBox, videoBox, offsetBox
            };

            var len = (uint)PiffWriter.GetBoxLength(this);
            offsetBox.MfraSize = len;
        }

        #endregion
    }
}
