using PiffLibrary.Boxes;

namespace PiffLibrary.Test
{
    internal class TestIntArray
    {
        [PiffDataFormat(PiffDataFormats.AsciiPascal)]
        public string Name { get; set; }

        public int[] Numbers { get; set; }
    }
}