namespace PiffLibrary.Boxes
{
    /// <summary>
    /// For audio:
    /// 0x40 - MPEG-4 AAC
    /// 0x66 - MPEG-2 AAC Main profile
    /// 0x67 - MPEG-2 AAC Low-complexity profile
    /// 0x68 - MPEG-2 AAC Scalable sampling rate profile
    /// 0x69 - MP3
    /// 0x6B - MPEG1 audio
    /// 0xA0 - EVRC voice
    /// 0xA1 - SMV voice
    /// 0xA5 - AC-3
    /// 0xA6 - Enhanced AC-3
    /// 0xA7 - DRA
    /// 0xA8 - ITU G719
    /// 0xA9 - DTS Coherent Acoustics
    /// 0xAA - DTS-HD High Resolution
    /// 0xAB - DTS-HD Master
    /// 0xE1 - QCELP 13K voice
    /// </summary>
    internal static class PiffAudioObjectTypes
    {
        public const byte Aac = 0x40;

        public const byte AacMain = 0x66;

        public const byte AacLc = 0x67;

        public const byte AacSsr = 0x68;

        public const byte Mp3 = 0x69;

        public const byte Mpeg1 = 0x6b;
    }
}
