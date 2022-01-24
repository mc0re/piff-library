using System.Collections.Generic;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Used to find sync samples.
    /// </summary>
    [BoxName("mfra")]
    [ChildType(typeof(PiffTrackFragmentRandomAccessBox))]
    [ChildType(typeof(PiffMovieFragmentRandomAccessOffsetBox))]
    public sealed class PiffMovieFragmentRandomAccessBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMovieFragmentRandomAccessBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffMovieFragmentRandomAccessBox(
            uint audioTrackId, IEnumerable<PiffSampleOffsetDto> audio,
            uint videoTrackId, IEnumerable<PiffSampleOffsetDto> video)
        {
            var audioBox = new PiffTrackFragmentRandomAccessBox(audioTrackId, audio);
            var videoBox = new PiffTrackFragmentRandomAccessBox(videoTrackId, video);
            var offsetBox = new PiffMovieFragmentRandomAccessOffsetBox();

            Children = new PiffBoxBase[]
            {
                audioBox, videoBox, offsetBox
            };

            var len = (uint) PiffWriter.GetBoxLength(this);
            offsetBox.MfraSize = len;
        }

        #endregion
    }
}
