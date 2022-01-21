namespace PiffLibrary.Boxes
{
    [BoxName("colr")]
    public sealed class PiffColourInformationBox : PiffBoxBase
    {
        /// <summary>
        /// Can be:
        /// - "nclx"
        /// - "rICC"
        /// - "prof"
        /// </summary>
        public string ColorType { get; set; }


        [PiffDataFormat(nameof(UseNclx))]
        public PiffColourOnScreenColoursProfile NclxProfile { get; set; }


        [PiffDataFormat(nameof(UseIccRestricted))]
        public PiffColourIccRestrictedProfile IccRestrictedProfile { get; set; }


        [PiffDataFormat(nameof(UseIccUnrestricted))]
        public PiffColourIccUnrestrictedProfile IccUnrestrictedProfile { get; set; }

        #region Format API

        private PiffDataFormats UseNclx() =>
            ColorType == "nclx" ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;
        

        private PiffDataFormats UseIccRestricted() =>
            ColorType == "rICC" ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;
        

        private PiffDataFormats UseIccUnrestricted() =>
            ColorType == "prof" ? PiffDataFormats.InlineObject : PiffDataFormats.Skip;
        
        #endregion
    }
}