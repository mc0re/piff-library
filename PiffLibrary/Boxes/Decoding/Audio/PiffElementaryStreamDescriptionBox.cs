using System;
using System.Linq;


namespace PiffLibrary.Boxes
{

    [BoxName("esds")]
    public sealed class PiffElementaryStreamDescriptionBox : PiffFullBoxBase
    {
        #region Properties

        public PiffDescriptorContainer Descriptor { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffElementaryStreamDescriptionBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffElementaryStreamDescriptionBox(ushort streamId, uint bitRate, int bufferSize, byte[] codecData)
        {
            Descriptor = new PiffDescriptorContainer
            {
                Tag = PiffEsdsBlockIds.Esd,
                Length = (uint) codecData.Length + 18 + 1 + 10,
                Esd = new PiffElementaryStreamDescriptor
                {
                    StreamId = streamId,
                },
                Children = new[]
                {
                    new PiffDescriptorContainer
                    {
                        Tag = PiffEsdsBlockIds.Dcd,
                        Length = (uint) codecData.Length + 18,
                        Dcd = new PiffDecoderConfigDescription
                        {
                            BufferSizeDb = bufferSize,
                            MaxBitRate = bitRate,
                            AverageBitRate = bitRate
                        },
                        Children = new[] { new PiffDescriptorContainer
                        {
                            Tag = PiffEsdsBlockIds.Dsi,
                            Length = (uint) codecData.Length,
                            Dsi = new PiffDecoderSpecificInfoDescriptor()
                            {
                                DsiData = codecData.ToArray()
                            }
                        }}
                    },
                    new PiffDescriptorContainer
                    {
                        Tag = PiffEsdsBlockIds.Slc,
                        Length = 1,
                        Slc = new PiffSyncLayerConfigDescription { PredefinedSync = 2 }
                    }
                }
            };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffElementaryStreamDescriptionBox Create(
            string codecId, ushort streamId, uint bitRate, int bufferSize, byte[] codecData)
        {
            if (codecId != "AACL")
                throw new ArgumentException($"Don't know how to deal with '{codecId}'.");

            return new PiffElementaryStreamDescriptionBox(streamId, bitRate, bufferSize, codecData);
        }

        #endregion
    }
}
