﻿namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Sizes of auxiliary information for each sample.
    /// There must be a matching <see cref="PiffSampleAuxiliaryOffsetBox"/> box with
    /// the same <see cref="AuxInfoType"/> and <see cref="AuxInfoTypeParameter"/>
    /// and the offsets for this auxiliary information.
    /// </summary>
    [BoxName("saiz")]
    public sealed class PiffSampleAuxiliaryInformationBox : PiffFullBoxBase
    {
        #region Properties

        /// <summary>
        /// Format of the auxiliary information.
        /// </summary>
        [PiffDataFormat(nameof(HasAuxType))]
        [PiffStringLength(4)]
        public string AuxInfoType { get; set; }


        /// <summary>
        /// Depends on <see cref="AuxInfoType"/>.
        /// </summary>
        [PiffDataFormat(nameof(HasAuxParameter))]
        public uint AuxInfoTypeParameter { get; set; }


        /// <summary>
        /// The auxiliary information size for each sample, if the same for all.
        /// 0 if variable - then <see cref="SampleInfoSize"/> is used.
        /// </summary>
        public byte DefaultSampleInfoSize { get; set; }


        /// <summary>
        /// The number of samples for which a size is defined.
        /// May be less than the number of samples in the track,
        /// then the remaining samples do not have any auxiliary information.
        /// </summary>
        public uint SampleCount { get; set; }


        /// <summary>
        /// The auxiliary information size for each sample.
        /// May be 0 if a sample has no auxiliary information.
        /// </summary>
        [PiffArraySize(nameof(SampleInfoCount))]
        public byte[] SampleInfoSize { get; set; }

        #endregion


        #region Format API

        private uint SampleInfoCount => DefaultSampleInfoSize == 0 ? SampleCount : 0;


        private PiffDataFormats HasAuxType() =>
            (Flags & 1) != 0 ? PiffDataFormats.Ascii : PiffDataFormats.Skip;


        private PiffDataFormats HasAuxParameter() =>
            (Flags & 1) != 0 ? PiffDataFormats.UInt32 : PiffDataFormats.Skip;

        #endregion
    }
}