namespace PiffLibrary
{
    [BoxName("mehd")]
    internal class PiffMovieExtendedHeader : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// When 1, use 64-bit duration. When 0 - 32-bit.
        /// </summary>
        public byte Version { get; } = 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        /// <summary>
        /// Length of the whole movie including fragments.
        /// </summary>
        public long Duration { get; }

        #endregion


        #region Init and clean-up

        public PiffMovieExtendedHeader(long duration)
        {
            Duration = duration;
        }

        #endregion
    }
}