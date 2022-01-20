namespace PiffLibrary.Boxes
{
    [BoxName("srpp")]
    [ChildType(typeof(PiffSchemeTypeBox))]
    [ChildType(typeof(PiffSchemeInformationBox))]
    public sealed class PiffSrtpProcessBox : PiffBoxBase
    {
        /// <summary>
        /// Algoriths are defined by SRTP.
        /// 
        /// "    " - algorithm is decided by a process outside the file format
        /// "ACM1" - AES Counter mode, 128-bit key
        /// "AF81" - AES F8 mode, 128-bit key
        /// "ENUL" - NULL encryption algorithm
        /// </summary>
        [PiffStringLength(4)]
        public string EncryptionAlgorithmRtp { get; set; }


        /// <summary>
        /// Algoriths are defined by SRTP.
        /// 
        /// "    " - algorithm is decided by a process outside the file format
        /// "ACM1" - AES Counter mode, 128-bit key
        /// "AF81" - AES F8 mode, 128-bit key
        /// "ENUL" - NULL encryption algorithm
        /// </summary>
        [PiffStringLength(4)]
        public string EncryptionAlgorithmRtcp { get; set;}


        /// <summary>
        /// Algoriths are defined by SRTP.
        /// 
        /// "    " - algorithm is decided by a process outside the file format
        /// "SHM2" - HMAC-SHA-1, 160-bit key
        /// "ANUL" - no integrity protection for RTP
        /// </summary>
        [PiffStringLength(4)]
        public string IntegrityAlgorithmRtp { get; set;}


        /// <summary>
        /// Algoriths are defined by SRTP.
        /// 
        /// "    " - algorithm is decided by a process outside the file format
        /// "SHM2" - HMAC-SHA-1, 160-bit key
        /// </summary>
        [PiffStringLength(4)]
        public string IntegrityAlgorithmRtcp { get; set;}
    }
}