using System.Collections.Generic;


namespace PiffLibrary
{
    /// <summary>
    /// Used to find sync samples.
    /// </summary>
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
            uint audioTrackId, IEnumerable<PiffSampleOffsetDto> audio,
            uint videoTrackId, IEnumerable<PiffSampleOffsetDto> video)
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
