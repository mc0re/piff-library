using System;
using System.Linq;


namespace PiffLibrary.Boxes
{
    [BoxName("avcC")]
    public sealed class PiffAvcConfigurationBox : PiffBoxBase
    {
        #region Constants

        private const byte NalUnitSize = 4;

        private const byte NalSequenceHeader = 0x67;

        private const byte NalPictureHeader = 0x68;

        #endregion


        #region Properties

        /// <summary>
        /// Must be 1.
        /// </summary>
        public byte ConfigurationVersion { get; set; }

        /// <summary>
        /// Profiles:
        ///  0x42: Baseline
	    ///  0x4D: Main
	    ///  0x53: Scalable Baseline
	    ///  0x56: Scalable High
	    ///  0x58: Extended
	    ///  0x64: High
	    ///  0x6E: High 10
	    ///  0x7A: High 4:2:2
	    ///  0x90, 0xF4: High 4:4:4
        /// </summary>
        public byte AvcProfile { get; set; }

        public byte ProfileCompatibility { get; set; }

        public byte AvcLevel { get; set; }


        [PiffDataFormat(PiffDataFormats.UInt6)]
        public byte Reserved1 { get; } = 0b111111;


        [PiffDataFormat(PiffDataFormats.UInt2Minus1)]
        public byte UnitSize { get; set; }


        #region Sequence properties

        [PiffDataFormat(PiffDataFormats.UInt3)]
        public byte Reserved2 { get; } = 0b111;


        /// <summary>
        /// The number of sequence parameters slots.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.UInt5)]
        public byte SequenceSlotCount { get; set; }


        /// <summary>
        /// Sequence parameters.
        /// </summary>
        [PiffArraySize(nameof(SequenceSlotCount))]
        public PiffAvcSequenceParameterItem[] SequenceSlots { get; set; }


        /// <summary>
        /// The number of picture parameters slots.
        /// </summary>
        public byte PictureSlotCount { get; set; }


        /// <summary>
        /// Picture parameters.
        /// </summary>
        [PiffArraySize(nameof(PictureSlotCount))]
        public PiffAvcPictureParameterItem[] PictureSlots { get; set; }

        #endregion

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffAvcConfigurationBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public PiffAvcConfigurationBox(string codecId, byte[] codecData)
        {
            if (codecId != "H264")
                throw new ArgumentException($"Cannot process codec '{codecId}', only 'H264' is supported.");

            var one = codecData.GetInt32(0);
            if (one != 1)
                throw new ArgumentException($"I don't know how to interpret number {one} at offset 0.");

            var seqId = codecData[4];
            if (seqId != NalSequenceHeader)
                throw new ArgumentException($"I don't know how to interpret header 0x{seqId:X} at offset 4.");

            ConfigurationVersion = 1;
            AvcProfile = codecData[5];
            ProfileCompatibility = codecData[6];
            AvcLevel = codecData[7];
            UnitSize = NalUnitSize;

            var seqEnd = 8;
            while (codecData.GetInt32(seqEnd) != 1)
                seqEnd++;

            SequenceSlotCount = 1;
            var seqSlot = new PiffAvcSequenceParameterItem
            {
                RawData = codecData.Skip(4).Take(seqEnd - 4).ToArray()
            };
            seqSlot.Length = (short) seqSlot.RawData.Length;
            SequenceSlots = new[] { seqSlot };

            var picId = codecData[seqEnd + 4];
            if (picId != NalPictureHeader)
                throw new ArgumentException($"I don't know how to interpret header 0x{picId:X} at offset {seqEnd + 4}.");

            PictureSlotCount = 1;
            var picSlot = new PiffAvcPictureParameterItem
            {
                RawData = codecData.Skip(seqEnd + 4).ToArray()
            };
            picSlot.Length = (short) picSlot.RawData.Length;
            PictureSlots = new[] { picSlot };
        }

        #endregion
    };
}
