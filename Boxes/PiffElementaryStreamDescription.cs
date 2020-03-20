using System.Linq;

namespace PiffLibrary
{
    [BoxName("esds")]
    internal class PiffElementaryStreamDescription
    {
        #region Properties

        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        #region Elementary Stream Descriptor block

        public byte EsdTag { get; } = PiffEsdsBlockIds.Esd;


        /// <summary>
        /// Length of ESD block's data (after the length field) and all its children - DCD, DSI, SLC.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int EsdLength { get; }


        /// <summary>
        /// Stream ID.
        /// </summary>
        public short EsId { get; }


        /// <summary>
        /// 0x80 - Stream dependant flag; if defined, read "short DependsOnEsId"
        /// 0x40 - URL; if defined, read URL Pascal-style, length 1 byte or 4 bytes
        /// 0x20 - OCR; if defined, read "short OcrEsId"
        /// Other 5 bits - stream priority
        /// </summary>
        public byte EsdFlags { get; } = 0;

        #endregion


        #region Decoder Config Descriptor block

        public byte DcdTag { get; } = PiffEsdsBlockIds.Dcd;


        /// <summary>
        /// Length of this block's data (after the Length field) and its child DSI.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int DcdLength { get; }


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
        public byte ObjectType { get; } = 0x40;


        /// <summary>
        /// 6 higher bits - stream type:
        /// - 5 (00101) - MPEG-4 audio stream
        /// 0x02 - upstream flag
        /// 0x01 - reserved (1)
        /// </summary>
        public byte StreamType { get; } = 0x15;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int BufferSizeDb { get; }


        public int MaxBitRate { get; }


        public int AverageBitRate { get; }

        #endregion


        #region Decoder Specific Info block

        public byte DsiTag { get; } = PiffEsdsBlockIds.Dsi;

        /// <summary>
        /// Codec data size.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int DsiLength { get; }


        /// <summary>
        /// Codec data.
        /// </summary>
        public byte[] DsiData { get; }

        #endregion


        #region Sync Layer Config Descriptor block

        public byte SlcTag { get; } = PiffEsdsBlockIds.Slc;


        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int SlcLength { get; } = 1;


        /// <summary>
        /// Defines a set of synchronization flags.
        /// 0 - Define the flags explicitly
        /// 1 - Null
        /// 2 - MP4
        /// </summary>
        public byte PredefinedSync { get; } = 2;

        #endregion

        #endregion


        #region Init and clean-up

        private PiffElementaryStreamDescription(short streamId, int bitRate, int bufferSize, byte[] codecData)
        {
            EsId = streamId;
            BufferSizeDb = bufferSize;
            MaxBitRate = bitRate;
            AverageBitRate = bitRate;
            DsiData = codecData.ToArray();
            DsiLength = DsiData.Length;
            DcdLength = DsiLength + 18;
            EsdLength = DsiLength + 32;
        }


        public static PiffElementaryStreamDescription Create(short streamId, int bitRate, int bufferSize, byte[] codecData)
        {
            return new PiffElementaryStreamDescription(streamId, bitRate, bufferSize, codecData);
        }

        #endregion
    }


    /// <summary>
    /// Object descriptor type ID.
    /// </summary>
    internal class PiffEsdsBlockIds
    {
        /// <summary>
        /// Elementary Stream Descriptor.
        /// </summary>
        public const byte Esd = 3;


        /// <summary>
        /// Decoder Config Descriptor.
        /// </summary>
        public const byte Dcd = 4;


        /// <summary>
        /// Decoder Specific Info.
        /// </summary>
        public const byte Dsi = 5;


        /// <summary>
        /// Sync Layer Config Descriptor.
        /// </summary>
        public const byte Slc = 6;
    }
}