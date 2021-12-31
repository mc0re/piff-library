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

            while (PiffReader.ReadBox(input, null, out var box) > 0)
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
