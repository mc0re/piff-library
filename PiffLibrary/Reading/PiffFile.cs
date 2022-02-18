using PiffLibrary.Boxes;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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


        /// <summary>
        /// Parse the given file. Keep all boxes but the "mdat" ones.
        /// </summary>
        /// <remarks>
        /// Limitations: we do not handle fragments stream.
        /// </remarks>
        public static PiffFile ParseButSkipData(Stream input, PiffReadContext ctx)
        {
            var file = new PiffFile();

            using (var bits = new BitReadStream(input, false))
            {
                while (true)
                {
                    var status = PiffReader.ReadBox(bits, ctx, out var box);
                    if (status == PiffReadStatuses.SkipToEnd) continue;
                    if (status != PiffReadStatuses.Continue) break;

                    // Bento4 code also delets "sidx" (keeping the first one) and "ssix" boxes.
                    var keep = true;

                    switch (box)
                    {
                        case PiffMediaDataBox _:
                            keep = false;
                            break;

                        default:
                            break;
                    }

                    if (keep)
                        file.Boxes.Add(box);
                }
            }

            return file;
        }


        /// <summary>
        /// Return the given box. Make sure it's the only one.
        /// </summary>
        public TBox GetSingleBox<TBox>() where TBox : PiffBoxBase =>
            Boxes.OfType<TBox>().Single();
    }
}
