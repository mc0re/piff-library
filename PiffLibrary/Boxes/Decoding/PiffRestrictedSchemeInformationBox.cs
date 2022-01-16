namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Container: Restricted Sample Entry "resv" box or Sample Entry box.
    /// </summary>
    [BoxName("rinf")]
    [ChildType(typeof(PiffOriginalFormatBox))]
    [ChildType(typeof(PiffSchemeTypeBox))]
    [ChildType(typeof(PiffSchemeInformationBox))]
    public sealed class PiffRestrictedSchemeInformationBox : PiffBoxBase
    {
    }
}