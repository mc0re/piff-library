using System;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// General extension box.
    /// </summary>
    [BoxName("uuid")]
    public sealed class PiffExtensionBox : PiffBoxBase
    {
        #region Properties

        /// <summary>
        /// This GUID corresponds to the PIFF protection box "PSSH".
        /// </summary>
        public Guid BoxId { get; set; }


        public byte Version { get; set; }


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; }


        /// <summary>
        /// Same as <see cref="PiffProtectionSystemSpecificHeaderBox"/>.
        /// Container: <see cref="PiffMovieBox"/>.
        /// </summary>
        [PiffDataFormat(nameof(UseProtectionInfo))]
        public PiffProtectionInfo Pssh { get; set; }


        /// <summary>
        /// Track encryption box.
        /// Container: <see cref="PiffSchemeInformationBox"/>.
        /// </summary>
        [PiffDataFormat(nameof(UseTrackEncryption))]
        public PiffProtectionTrackEncryption Track { get; set; }


        /// <summary>
        /// Same as <see cref="PiffSampleEncryptionBox"/>.
        /// Container: <see cref="PiffTrackFragmentBox"/>.
        /// </summary>
        [PiffDataFormat(nameof(UseSampleEncryption))]
        public PiffSampleEncryption Sample { get; set; }


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
            var box = new PiffExtensionBox
            {
                BoxId = PiffProtectionInfo.BoxId
            };

            box.Pssh = new PiffProtectionInfo(box)
            {
                SystemId = systemId,
                BinData = data,
                DataSize = (uint) data.Length
            };

            return box;
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

        private PiffDataFormats UseProtectionInfo() =>
            BoxId == PiffProtectionInfo.BoxId ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats UseTrackEncryption() =>
            BoxId == PiffProtectionTrackEncryption.BoxId ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;


        private PiffDataFormats UseSampleEncryption() =>
            BoxId == PiffSampleEncryption.BoxId ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;

        #endregion
    }
}
