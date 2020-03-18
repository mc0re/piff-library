using System;

namespace PiffLibrary
{
    [BoxName("mdia")]
    internal class PIffTrackMediaInfo
    {
        #region Properties

        public PiffMediaHeader Header { get; }

        public PiffHandlerType Handler { get; }

        public PiffMediaInformation Info { get; }

        #endregion


        #region Init and clean-up

        private PIffTrackMediaInfo(PiffMediaHeader header, PiffHandlerType handler, PiffMediaInformation info)
        {
            Header = header;
            Handler = handler;
            Info = info;
        }


        public static PIffTrackMediaInfo CreateAudio(
            short channels, short bitsPerSample, ushort samplingRate,
            short streamId, int bitRate, byte[] codecData,
            DateTime created, TimeSpan duration, int timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfo(
                new PiffMediaHeader(created, duration, timeScale),
                new PiffHandlerType(PiffTrackTypes.Audio),
                PiffMediaInformation.CreateAudio(channels, bitsPerSample, samplingRate, streamId, bitRate, codecData, keyId));
        }


        public static PIffTrackMediaInfo CreateVideo(
            short width, short height, DateTime created, TimeSpan duration, int timeScale, Guid keyId)
        {
            return new PIffTrackMediaInfo(new PiffMediaHeader(created, duration, timeScale),
                                          new PiffHandlerType(PiffTrackTypes.Video),
                                          PiffMediaInformation.CreateVideo(width, height, keyId));
        }

        #endregion
    }
}