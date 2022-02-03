using System;
using System.Linq;


namespace PiffLibrary.Boxes
{
    [BoxName("esds")]
    public sealed class PiffElementaryStreamDescriptionMp4aBox : PiffFullBoxBase
    {
        #region Properties

        #region Elementary Stream Descriptor block

        public byte EsdTag { get; } = PiffEsdsBlockIds.Esd;


        /// <summary>
        /// Length of ESD block's data (after the length field) and all its children - DCD, DSI, SLC.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int EsdLength { get; set; }


        /// <summary>
        /// Stream ID.
        /// </summary>
        public short EsId { get; set; }


        /// <summary>
        /// 0x80 - Stream dependant flag; if defined, read "short DependsOnEsId"
        /// 0x40 - URL; if defined, read URL Pascal-style, length 1 byte or 4 bytes
        /// 0x20 - OCR; if defined, read "short OcrEsId"
        /// Other 5 bits - stream priority
        /// </summary>
        public byte EsdFlags { get; set; }

        #endregion


        #region Decoder Config Descriptor block

        public byte DcdTag { get; } = PiffEsdsBlockIds.Dcd;


        /// <summary>
        /// Length of this block's data (after the Length field) and its child DSI.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int DcdLength { get; set; }


        /// <summary>
        /// Type of audio.
        /// </summary>
        public byte ObjectType { get; set; } = PiffAudioObjectTypes.Aac;


        /// <summary>
        /// 6 higher bits - stream type:
        /// - 5 (00101) - MPEG-4 audio stream
        /// 0x02 - upstream flag
        /// 0x01 - reserved (1)
        /// </summary>
        public byte StreamType { get; set; } = 5 << 2 | 1;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int BufferSizeDb { get; set; }


        public int MaxBitRate { get; set; }


        public int AverageBitRate { get; set; }

        #endregion


        #region Decoder Specific Info block

        public byte DsiTag { get; } = PiffEsdsBlockIds.Dsi;

        /// <summary>
        /// Codec data size.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.DynamicInt)]
        public int DsiLength { get; set; }


        /// <summary>
        /// Codec data.
        /// </summary>
        [PiffArraySize(nameof(DsiLength))]
        public byte[] DsiData { get; set; }

        #endregion


        #region Sync Layer Config Descriptor block

        public byte SlcTag { get; } = PiffEsdsBlockIds.Slc;


        /// <summary>
        /// According to the standard, this should be <see cref="PiffDataFormats.DynamicInt"/>
        /// just as the other lengths. But it can never take more than 7 bits,
        /// so let's write it as a <see langword="byte"/> instead.
        /// Aligned with the sample stream.
        /// </summary>
        public byte SlcLength { get; set; } = 1;


        /// <summary>
        /// Defines a set of synchronization flags.
        /// 0 - Define the flags explicitly
        /// 1 - Null
        /// 2 - MP4
        /// </summary>
        public byte PredefinedSync { get; set; } = 2;

        #endregion

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffElementaryStreamDescriptionMp4aBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffElementaryStreamDescriptionMp4aBox(short streamId, int bitRate, int bufferSize, byte[] codecData)
        {
            EsId = streamId;
            BufferSizeDb = bufferSize;
            MaxBitRate = bitRate;
            AverageBitRate = bitRate;
            DsiData = codecData.ToArray();
            DsiLength = DsiData.Length;

            // Structure:
            // ESD { DCD { DSI }, SLC }
            DcdLength = DsiLength + 18;
            EsdLength = 3 + DsiLength + 18 + SlcLength + 1 + 6;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffElementaryStreamDescriptionMp4aBox Create(
            string codecId, short streamId, int bitRate, int bufferSize, byte[] codecData)
        {
            if (codecId != "AACL")
                throw new ArgumentException($"Don't know how to deal with '{codecId}'.");

            return new PiffElementaryStreamDescriptionMp4aBox(streamId, bitRate, bufferSize, codecData);
        }

        #endregion
    }
}
