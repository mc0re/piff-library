using System;


namespace PiffLibrary
{
    /// <summary>
    /// General extension box.
    /// </summary>
    [BoxName("uuid")]
    internal class PiffExtensionBox : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// This GUID corresponds to the PIFF protection box "PSSH".
        /// </summary>
        public Guid BoxId { get; set; }


        public byte Version { get; set; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        [PiffDataFormat(nameof(UseProtectionInfo))]
        public PiffProtectionInfo Pssh { get; set; }


        [PiffDataFormat(nameof(UseTrackEncryption))]
        public PiffProtectionTrackEncryption Track { get; set; }


        /// <summary>
        /// Catches all data not collected by the previous objects.
        /// I.e. if the box ID was not recognized.
        /// </summary>
        public byte[] Data { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffExtensionBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffExtensionBox ProtectionInfo(Guid systemId, byte[] data)
        {
            return new PiffExtensionBox
            {
                BoxId = PiffProtectionInfo.BoxId,
                Pssh = new PiffProtectionInfo
                {
                    SystemId = systemId,
                    BinData = data,
                    DataSize = data.Length
                }
            };
        }

        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffExtensionBox ProtectionTrackEncryption(Guid keyId)
        {
            return new PiffExtensionBox
            {
                BoxId = PiffProtectionTrackEncryption.BoxId,
                Track = new PiffProtectionTrackEncryption
                {
                    ContentKeyId = keyId
                }
            };
        }
        #endregion


        #region Format API

        public PiffDataFormats UseProtectionInfo() =>
            BoxId == PiffProtectionInfo.BoxId ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        public PiffDataFormats UseTrackEncryption() =>
            BoxId == PiffProtectionTrackEncryption.BoxId ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}
