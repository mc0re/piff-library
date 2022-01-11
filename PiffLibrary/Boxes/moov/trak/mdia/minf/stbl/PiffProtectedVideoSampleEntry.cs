﻿using System;


namespace PiffLibrary
{
    [BoxName("encv")]
    [ChildType(typeof(PiffProtectionSchemeInformation))]
    [ChildType(typeof(PiffAvcConfiguration))]
    internal class PiffProtectedVideoSampleEntry : PiffBoxBase
    {
        #region Properties

        [PiffArraySize(6)]
        public byte[] Reserved1 { get; } = { 0, 0, 0, 0, 0, 0 };


        /// <summary>
        /// Index to the data reference.
        /// </summary>
        public short DataReferenceIndex { get; set; } = 1;


        [PiffArraySize(2)]
        public short[] Reserved2 { get; } = { 0, 0 };


        [PiffArraySize(3)]
        public int[] Reserved3 { get; } = { 0, 0, 0 };


        public ushort Width { get; set; }


        public ushort Height { get; set; }


        /// <summary>
        /// 72 dpi.
        /// </summary>
        public int HorizontalResolution { get; set; } = 0x480000;


        /// <summary>
        /// 72 dpi.
        /// </summary>
        public int VerticalResolution { get; set; } = 0x480000;


        public int Reserved4 { get; }


        public short FrameCount { get; set; } = 1;


        /// <summary>
        /// Fixed-size field for display purposes.
        /// </summary>
        [PiffArraySize(32)]
        public byte[] CompressorName { get; set; } = new byte[32];


        public short Depth { get; set; } = 0x18;


        public short Reserved5 { get; } = -1;

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectedVideoSampleEntry()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffProtectedVideoSampleEntry(PiffVideoManifest video, Guid keyId)
        {
            Width = video.Width;
            Height = video.Height;
            var scheme = PiffProtectionSchemeInformation.CreateVideo(video.CodecId, keyId);
            var config = new PiffAvcConfiguration(video.CodecId, video.CodecData);
            Childen = new PiffBoxBase[] {scheme, config};
        }

        #endregion
    }
}