using PiffLibrary.Boxes;
using System.Collections.Generic;
using System.IO;


namespace PiffLibrary
{
    [ChildType(typeof(PiffFileTypeBox))]
    [ChildType(typeof(PiffProgressiveDownloadBox))]
    [ChildType(typeof(PiffMovieBox))]
    [ChildType(typeof(PiffMovieFragmentBox))]
    [ChildType(typeof(PiffMovieFragmentRandomAccessBox))]
    [ChildType(typeof(PiffMetadataBox))]
    [ChildType(typeof(PiffMetadataContainerBox))]
    [ChildType(typeof(PiffSkipBox))]
    public sealed class PiffFile
    {
        private readonly List<PiffBoxBase> mBoxes = new List<PiffBoxBase>();

        private PiffFileTypeBox mFileTypeBox;


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
                    file.mBoxes.Add(box);

                    switch (box)
                    {
                        case PiffFileTypeBox ftyp:
                            file.mFileTypeBox = ftyp;
                            break;
                    }
                }
            }

            return file;
        }
    }
}
