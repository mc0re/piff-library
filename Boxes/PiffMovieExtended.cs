using System;

namespace PiffLibrary
{
    [BoxName("mvex")]
    internal class PiffMovieExtended
    {
        #region Properties

        public PiffMovieExtendedHeader Header { get; }

        public PiffTrackExtended[] Track { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieExtended(TimeSpan duration, int timeScale)
        {
            Header = new PiffMovieExtendedHeader(duration, timeScale);
            Track = new[]
            {
                new PiffTrackExtended(1),
                new PiffTrackExtended(2)
            };
        }

        #endregion
    }
}