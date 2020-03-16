using System;

namespace PiffLibrary
{
    [BoxName("encv")]
    internal class PiffProtectedVideoSampleEntry
    {
        #region Properties

        public byte[] Reserved1 { get; } = { 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Index to the data reference.
        /// </summary>
        public short DataReferenceIndex { get; } = 1;

        public short[] Reserved2 { get; } = { 0, 0 };

        public int[] Reserved3 { get; } = { 0, 0, 0 };

        public short Width { get; }

        public short Height { get; }

        /// <summary>
        /// 72 dpi.
        /// </summary>
        public int HorizontalResolution { get; } = 0x480000;

        /// <summary>
        /// 72 dpi.
        /// </summary>
        public int VerticalResolution { get; } = 0x480000;

        public int Reserved4 { get; } = 0;

        public short FrameCount { get; } = 1;

        /// <summary>
        /// FIxed-size field for display purposes.
        /// </summary>
        public byte[] CompressorName { get; } = new byte[32];

        public short Depth { get; } = 0x18;

        public short Reserved5 { get; } = -1;


        public PiffAvcConfiguration AvcConfiguration { get; } = new PiffAvcConfiguration();

        public PiffProtectionSchemeInformation Scheme { get; }

        #endregion


        #region Init and clean-up

        public PiffProtectedVideoSampleEntry(short width, short height, Guid keyId)
        {
            Width = width;
            Height = height;
            Scheme = PiffProtectionSchemeInformation.CreateVideo(keyId);
        }

        #endregion
    }
}