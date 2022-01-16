namespace PiffLibrary.Boxes
{
    [BoxName("rinf")]
    [ChildType(typeof(PiffOriginalFormatBox))]
    [ChildType(typeof(PiffSchemeTypeBox))]
    [ChildType(typeof(PiffSchemeInformationBox))]
    public sealed class PiffRestrictedSchemeInformationBox : PiffBoxBase
    {

    }
}