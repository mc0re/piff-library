using System;

namespace PiffLibrary
{
    [BoxName("uuid")]
    internal class PiffProtectionInfo
    {
        #region Properties

        /// <summary>
        /// This GUID corresponds to the protection box.
        /// </summary>
        public Guid BoxId { get; } = Guid.Parse("d08a4f18-10f3-4a82-b6c8-32d8aba183d3");


        public byte Version { get; } = 0;


        [PiffDataFormat(PiffDataFormats.Int24)]
        public int Flags { get; } = 0;


        public Guid SystemId { get; }


        /// <summary>
        /// Total size of the following properties, except for <see cref="DataSize"/> itself.
        /// </summary>
        public int DataSize { get; }


        /// <summary>
        /// First 10 bytes are binary data.
        /// Then XML in Unicode comes in:
        /// &lt;WRMHEADER&gt;...&lt;/WRMHEADER&gt;
        /// 
        /// - WRMHEADER/DATA/KID - Base-64 encoded GUID
        /// - WRMHEADER/DATA/LA_URL - License aquisition URL
        /// - WRMHEADER/DATA/DS_ID - Base-64 encoded domain service GUID
        /// 
        /// Used by Windows.Media.Protection.PlayReady.PlayReadyLicenseAcquisitionServiceRequest
        /// </summary>
        public byte[] BinData { get; }

        #endregion


        #region Init and clean-up

        public PiffProtectionInfo(Guid systemId, byte[] data)
        {
            SystemId = systemId;
            BinData = data;
            DataSize = data.Length;
        }

        #endregion
    }
}