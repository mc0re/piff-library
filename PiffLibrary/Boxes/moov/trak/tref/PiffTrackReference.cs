using PiffLibrary.Boxes;

namespace PiffLibrary
{
    [BoxName("tref")]
    [ChildType(typeof(PiffTrackReferenceHint))]
    [ChildType(typeof(PiffTrackReferenceHintDependency))]
    [ChildType(typeof(PiffTrackReferenceDescription))]
    [ChildType(typeof(PiffTrackReferenceFont))]
    [ChildType(typeof(PiffTrackReferenceDepth))]
    [ChildType(typeof(PiffTrackReferenceParallax))]
    [ChildType(typeof(PiffTrackReferenceOverlay))]
    internal class PiffTrackReference : PiffBoxBase
    {
    }
}