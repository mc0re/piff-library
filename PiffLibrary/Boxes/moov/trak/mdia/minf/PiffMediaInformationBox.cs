using System;


namespace PiffLibrary.Boxes
{
    [BoxName("minf")]
    [ChildType(typeof(PiffNullMediaHeaderBox))]
    [ChildType(typeof(PiffSampleTableBox))]
    [ChildType(typeof(PiffDataInformationBox))]

    [ChildType(typeof(PiffSoundMediaHeaderBox))]
    [ChildType(typeof(PiffVideoMediaHeaderBox))]
    public sealed class PiffMediaInformationBox : PiffBoxBase
    {
        #region Init and clean-up

        /// <summary>
        /// Constructor for reading.
        /// </summary>
        public PiffMediaInformationBox()
        {
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        private PiffMediaInformationBox(PiffSoundMediaHeaderBox sound, PiffVideoMediaHeaderBox video, PiffSampleTableBox index)
        {
            // There must be a dinf box
            var dinf = new PiffDataInformationBox
            {
                Children = new PiffBoxBase[]
                {
                    new PiffDataReferenceBox
                    {
                        Count = 1,
                        Children = new PiffBoxBase[]
                        {
                            new PiffDataEntryUrlBox { Flags = 1 }
                        }
                    }
                }
            };

            var track = (PiffBoxBase) sound ?? video;
            Children = new PiffBoxBase[] { track, dinf, index };
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffMediaInformationBox CreateAudio(PiffAudioManifest audio, Guid keyId)
        {
            return new PiffMediaInformationBox(new PiffSoundMediaHeaderBox(),
                                            null,
                                            PiffSampleTableBox.CreateAudio(audio, keyId));
        }


        /// <summary>
        /// Constructor for writing.
        /// </summary>
        public static PiffMediaInformationBox CreateVideo(PiffVideoManifest video, Guid keyId)
        {
            return new PiffMediaInformationBox(null, new PiffVideoMediaHeaderBox(),
                                            PiffSampleTableBox.CreateVideo(video, keyId));
        }

        #endregion
    }
}