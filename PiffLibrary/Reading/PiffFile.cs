using PiffLibrary.Boxes;
using System.Collections.Generic;
using System.IO;


namespace PiffLibrary
{
    [ChildType(typeof(PiffFileTypeBox))]
    [ChildType(typeof(PiffSegmentTypeBox))]
    [ChildType(typeof(PiffSegmentIndexBox))]
    [ChildType(typeof(PiffSubsegmentIndexBox))]
    [ChildType(typeof(PiffProgressiveDownloadBox))]
    [ChildType(typeof(PiffMovieBox))]
    [ChildType(typeof(PiffMovieFragmentBox))]
    [ChildType(typeof(PiffMovieFragmentRandomAccessBox))]
    [ChildType(typeof(PiffMetadataBox))]
    [ChildType(typeof(PiffMetadataContainerBox))]
    [ChildType(typeof(PiffMediaDataBox))]
    [ChildType(typeof(PiffProducerReferenceTimeBox))]
    [ChildType(typeof(PiffSkipBox))]
    public sealed class PiffFile
    {
        public List<PiffBoxBase> Boxes { get; } = new List<PiffBoxBase>();


        private PiffFile()
        {
        }


        public static PiffFile Parse(Stream input)
        {
            var file = new PiffFile();
            var ctx = new PiffReadContext();
            using (var bits = new BitReadStream(input, false))
            {
                while (PiffReader.ReadBox(bits, ctx, out var box) > 0 && box != null)
                {
                    switch (box)
                    {
                        case PiffMediaDataBox _:
                            // Skip
                            break;

                        default:
                            file.Boxes.Add(box);
                            break;
                    }
                }
            }

            return file;
        }
    }
}
