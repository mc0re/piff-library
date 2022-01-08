using System.Collections.Generic;
using System.IO;


namespace PiffLibrary
{
    public class PiffFile
    {
        private readonly List<PiffBoxBase> mBoxes = new List<PiffBoxBase>();

        private PiffFileType mFileTypeBox;


        private PiffFile()
        {
        }


        public static PiffFile Parse(Stream input)
        {
            var file = new PiffFile();
            var ctx = new PiffReadContext();

            while (PiffReader.ReadBox(input, ctx, out var box) > 0 && box != null)
            {
                file.mBoxes.Add(box);

                switch (box)
                {
                    case PiffFileType ftyp:
                        file.mFileTypeBox = ftyp;
                        break;
                }
            }

            return file;
        }
    }
}
