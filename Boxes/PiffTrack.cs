using System;

namespace PiffLibrary
{
    [BoxName("trak")]
    internal class PiffTrack
    {
        #region Properties

        public PiffTrackHeader Header { get; }

        public PIffTrackMediaInfo Media { get; }

        #endregion


        #region Init and clean-up

        private PiffTrack(PiffTrackHeader header, PIffTrackMediaInfo info)
        {
            Header = header;
            Media = info;
        }


        public static PiffTrack CreateAudio(
            short channels, short bitsPerSample, ushort samplingRate,
            byte trackId, int bitRate, byte[] codecData,
            DateTime created, TimeSpan duration, int timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateAudio(trackId, created, duration, timeScale),
                                 PIffTrackMediaInfo.CreateAudio(channels, bitsPerSample, samplingRate, trackId, bitRate, codecData, created, duration, timeScale, keyId));
        }


        public static PiffTrack CreateVideo(
            short width, short height, DateTime created, TimeSpan duration, int timeScale, Guid keyId)
        {
            return new PiffTrack(PiffTrackHeader.CreateVideo(2, created, duration, timeScale, width, height),
                                 PIffTrackMediaInfo.CreateVideo(width, height, created, duration, timeScale, keyId));
        }

        #endregion
    }
}