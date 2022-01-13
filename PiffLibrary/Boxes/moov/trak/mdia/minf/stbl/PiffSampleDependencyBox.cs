namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Whether a sample is an I-frame and other dependencies.
    /// </summary>
    [BoxName("sdtp")]
    public sealed class PiffSampleDependencyBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// The number of samples is given in 'stsz' or 'stz2' box.
        /// 
        /// For each sample:
        /// - 2 bits - is leading
        ///   - 00 - unknown
        ///   - 01 - leading with dependency before (i.e. is not decodable)
        ///   - 10 - not leading
        ///   - 11 - leading with no prior dependency (i.e. is decodable)
        ///   
        /// - 2 bits - depends on
        ///   - 00 - unknown
        ///   - 01 - depends on other samples (i.e. is not an I-frame)
        ///   - 10 - does not depend on other samples (i.e. is an I-frame)
        ///   - 11 - reserved
        ///   
        /// - 2 bits - is depended on
        ///   - 00 - unknown
        ///   - 01 - other samples may depend on this one (i.e. is not disposable)
        ///   - 10 - no other samples depend on this one (i.e. is disposable)
        ///   - 11 - reserved
        ///   
        /// - 2 bits - has redundancy
        ///   - 00 - unknown
        ///   - 01 - there is redundant coding in this sample
        ///   - 10 - there is no redundant coding in this sample
        ///   - 11 - reserved
        /// </summary>
        public byte[] Dependencies { get; set; }

        #endregion
    }
}