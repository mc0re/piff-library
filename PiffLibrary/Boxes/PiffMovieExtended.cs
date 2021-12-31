namespace PiffLibrary
{
    [BoxName("mvex")]
    internal class PiffMovieExtended : PiffBoxBase
    {
        #region Properties

        public PiffMovieExtendedHeader Header { get; }

        public PiffTrackExtended[] Track { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieExtended(long duration)
        {
            Header = new PiffMovieExtendedHeader(duration);
            Track = new[]
            {
                new PiffTrackExtended(1),
                new PiffTrackExtended(2)
            };
        }

        #endregion
    }
}