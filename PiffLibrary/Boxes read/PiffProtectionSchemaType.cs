﻿namespace PiffLibrary
{
    [BoxName("schm")]
    internal class PiffProtectionSchemaType : PiffBoxBase
    {
        #region Properties

        public byte Version { get; set; } = 0;


        /// <summary>
        /// Whether the box contains schema installation URL.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; set; } = 0;


        [PiffDataLength(4)]
        public string SchemaType { get; set; } = "piff";


        public int SchemaVersion { get; set; }


        [PiffDataFormat(nameof(GetUrlFormat))]
        public string SchemaUrl { get; set; }

        #endregion


        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffProtectionSchemaType()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffProtectionSchemaType(int version)
        {
            SchemaVersion = version;
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemaType CreateAudio()
        {
            return new PiffProtectionSchemaType(0x10000);
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffProtectionSchemaType CreateVideo()
        {
            return new PiffProtectionSchemaType(0x10001);
        }

        #endregion


        #region Format API

        public PiffDataFormats GetUrlFormat()
        {
            return (Flags & 1) == 1 ? PiffDataFormats.Utf8Zero : PiffDataFormats.Skip;
        }

        #endregion
    }
}