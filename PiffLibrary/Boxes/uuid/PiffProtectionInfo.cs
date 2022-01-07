﻿using System;


namespace PiffLibrary
{
    /// <summary>
    /// PlayReady uses "pssh" extension box for protection information.
    /// </summary>
    internal class PiffProtectionInfo
    {
        #region Constants

        public static readonly Guid BoxId = Guid.Parse("d08a4f18-10f3-4a82-b6c8-32d8aba183d3");

        #endregion


        #region Properties

        /// <summary>
        /// PSSH system ID.
        /// PlayReady: 9a04f079-9840-4286-ab92-e65be0885f95.
        /// </summary>
        public Guid SystemId { get; set; }


        /// <summary>
        /// Total size of the following properties, except for <see cref="DataSize"/> itself.
        /// </summary>
        public int DataSize { get; set; }


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
        [PiffArraySize(nameof(DataSize))]
        public byte[] BinData { get; set; }// = new byte[0];

        #endregion
    }
}