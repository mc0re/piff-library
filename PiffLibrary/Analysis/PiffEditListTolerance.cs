using System;

namespace PiffLibrary
{
    /// <summary>
    /// Media time, in real time, below which an edit list is trivial, per track type.
    /// </summary>
    public sealed class PiffEditListTolerance
    {
        #region Properties

        /// <summary>
        /// Tolerance for video tracks and for tracks of another or unknown handler type.
        /// </summary>
        public TimeSpan Video { get; }


        /// <summary>
        /// Tolerance for audio tracks (handler type "soun"), e.g. for AAC encoder priming.
        /// </summary>
        public TimeSpan Audio { get; }

        #endregion


        #region Init and clean-up

        public PiffEditListTolerance(TimeSpan video, TimeSpan audio)
        {
            Video = video;
            Audio = audio;
        }


        /// <summary>
        /// The same tolerance for all track types.
        /// </summary>
        public PiffEditListTolerance(TimeSpan all)
            : this(all, all)
        {
        }

        #endregion


        #region API

        /// <summary>
        /// Tolerance for a track of the given handler type ("vide", "soun", ...); <see langword="null"/> is unknown.
        /// </summary>
        public TimeSpan For(string handlerType) =>
            handlerType == PiffEditListInfo.AudioHandlerType ? Audio : Video;


        public override string ToString() => $"video {Video.TotalMilliseconds} ms, audio {Audio.TotalMilliseconds} ms";

        #endregion
    }
}
