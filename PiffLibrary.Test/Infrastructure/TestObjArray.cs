using PiffLibrary.Boxes;

namespace PiffLibrary.Test
{
    internal class TestObjArray
    {
        [PiffDataFormat(PiffDataFormats.AsciiZero)]
        public string? Name { get; set; }

        public TestObjArrayItem[]? Items { get; set; }
    }
}