namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Audio type.
    /// </summary>
    internal static class PiffAudioObjectTypes
    {
        public const byte Mpeg4Aac = 0x40;

        public const byte Mpeg2AacMain = 0x66;

        public const byte Mpeg2AacLowComplexity = 0x67;

        public const byte Mpeg2AacScalableSamplingRate = 0x68;

        public const byte Mp3 = 0x69;

        public const byte Mpeg1 = 0x6b;
        
        public const byte EvrcVoice = 0xa0;
        
        public const byte SmvVoice = 0xa1;
        
        public const byte Ac3 = 0xa5;
        
        public const byte EAc3 = 0xa6;
        
        public const byte Dra = 0xa7;
        
        public const byte G719 = 0xa8;
        
        public const byte DtsCoherentAcoustics = 0xa9;
        
        public const byte DtsHdHiRes = 0xaa;
        
        public const byte DtsHdMaster = 0xab;
        
        public const byte QcelpVoice = 0xe1;
    }
}
