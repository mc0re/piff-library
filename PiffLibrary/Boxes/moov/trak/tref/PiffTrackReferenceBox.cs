namespace PiffLibrary.Boxes
{
    [BoxName("tref")]
    [ChildType(typeof(PiffTrackReferenceHintBox))]
    [ChildType(typeof(PiffTrackReferenceHintDependencyBox))]
    [ChildType(typeof(PiffTrackReferenceDescriptionBox))]
    [ChildType(typeof(PiffTrackReferenceFontBox))]
    [ChildType(typeof(PiffTrackReferenceDepthBox))]
    [ChildType(typeof(PiffTrackReferenceParallax))]
    [ChildType(typeof(PiffTrackReferenceOverlayBox))]
    public sealed class PiffTrackReferenceBox : PiffBoxBase
    {
    }
}