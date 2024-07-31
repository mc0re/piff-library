using System;
using System.Text;


namespace PiffLibrary.Boxes
{
    public sealed class PiffProtectionInfo
    {
        #region Constants

        public static readonly Guid BoxId = Guid.Parse("d08a4f18-10f3-4a82-b6c8-32d8aba183d3");

        public static readonly Guid PlayReadySystemId = Guid.Parse("9a04f079-9840-4286-ab92-e65be0885f95");

        #endregion


        #region Fields

        private byte mParentVersion;

        #endregion


        #region Properties

        /// <summary>
        /// PSSH system ID.
        /// Recognized only <see cref="PlayReadySystemId"/>.
        /// </summary>
        public Guid SystemId { get; set; }


        [PiffDataFormat(nameof(UseKids))]
        public uint KidCount { get; set; }


        [PiffArraySize(nameof(KidCount))]
        public Guid[] Kids { get; set; }


        /// <summary>
        /// Total size of the following properties, except for <see cref="DataSize"/> itself.
        /// </summary>
        public uint DataSize { get; set; }


        /// <summary>
        /// First 10 bytes are some numbers in little endian format:
        /// - 4 bytes block length
        /// - 2 bytes value 1
        /// - 2 bytes value 1
        /// - 2 bytes payload length
        /// 
        /// Then XML in Unicode comes in (see <see cref="CreateWrmHeader"/>).
        /// Used by Windows.Media.Protection.PlayReady.PlayReadyLicenseAcquisitionServiceRequest
        /// </summary>
        [PiffArraySize(nameof(DataSize))]
        public byte[] BinData { get; set; }

        #endregion


        #region Init and clean-up

        public PiffProtectionInfo(PiffProtectionSystemSpecificHeaderBox parent)
        {
            mParentVersion = parent.Version;
        }


        public PiffProtectionInfo(PiffExtensionBox parent)
        {
            mParentVersion = parent.Version;
        }

        #endregion


        #region API

        /// <summary>
        /// Create a protection XML.
        /// </summary>
        /// <param name="keyLength">Length of the key value.</param>
        /// <param name="algId">Encryption algorithm.</param>
        /// <param name="keyId">Key ID (base-64 encoded `Guid.ToByteArray()`).</param>
        /// <param name="checkSum">Key ID encrypted with key value (see PlayReadyRightsManagementHeader), base-64.</param>
        /// <param name="acquisitionUrl">URL for PlayReady.</param>
        /// <returns>The XML to incorporate into the video file.</returns>
        public static string CreateWrmHeader(int keyLength, string algId, string keyId, string checkSum, string acquisitionUrl) =>
            $"<WRMHEADER version=\"4.0.0.0\" xmlns=\"http://schemas.microsoft.com/DRM/2007/03/PlayReadyHeader\"><DATA><PROTECTINFO><KEYLEN>{keyLength}</KEYLEN><ALGID>{algId}</ALGID></PROTECTINFO><KID>{keyId}</KID><CHECKSUM>{checkSum}</CHECKSUM><LA_URL>{acquisitionUrl}</LA_URL></DATA></WRMHEADER>";


        /// <summary>
        /// Create <see cref="BinData"/> content off the given XML payload.
        /// </summary>
        public static byte[] CreateBinData(string payload)
        {
            var strLen = payload.Length * 2; // Unicode
            var res = new byte[strLen + 10];

            Array.Copy(BitConverter.GetBytes(strLen + 10), res, 4);
            Array.Copy(new byte[] {1, 0, 1, 0}, 0, res, 4, 4);
            Array.Copy(BitConverter.GetBytes((short)strLen), 0, res, 8, 2);
            Encoding.Unicode.GetBytes(payload, 0, payload.Length, res, 10);

            return res;
        }

        #endregion


        #region Format

        private PiffDataFormats UseKids() =>
            mParentVersion > 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}