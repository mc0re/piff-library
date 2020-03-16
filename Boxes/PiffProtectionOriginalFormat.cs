namespace PiffLibrary
{
    [BoxName("frma")]
    internal class PiffProtectionOriginalFormat
    {
        #region Properties

        public string Format { get; }

        #endregion


        #region Init and clean-up

        private PiffProtectionOriginalFormat(string format)
        {
            Format = format;
        }


        public static PiffProtectionOriginalFormat CreateAudio()
        {
            return new PiffProtectionOriginalFormat("mp4a");
        }


        public static PiffProtectionOriginalFormat CreateVideo()
        {
            return new PiffProtectionOriginalFormat("avc1");
        }

        #endregion
    }
}