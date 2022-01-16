﻿namespace PiffLibrary.Boxes
{
    /// <summary>
    /// User information about the containing box.
    /// </summary>
    [BoxName("udta")]
    [ChildType(typeof(PiffCopyrightBox))]
    [ChildType(typeof(PiffTrackSelectionBox))]
    [ChildType(typeof(PiffTrackKindBox))]
    [ChildType(typeof(PiffSubTrackBox))]
    public sealed class PiffUserDataBox : PiffBoxBase
    {
    }
}
