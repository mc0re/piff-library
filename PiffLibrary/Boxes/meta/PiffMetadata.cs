using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("meta")]
    [ChildType(typeof(PiffHandlerType))]
    [ChildType(typeof(PiffDataInformationBox))]
    internal class PiffMetadata : PiffBoxBase
    {

    }
}
