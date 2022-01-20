namespace PiffLibrary.Boxes
{
    [BoxName("hinf")]
    [ChildType(typeof(PiffHintTotalBytesSentBox))]
    [ChildType(typeof(PiffHintTotalBytesSent64Box))]
    [ChildType(typeof(PiffHintPacketsSentBox))]
    [ChildType(typeof(PiffHintPacketsSent64Box))]
    [ChildType(typeof(PiffHintBytesSentBox))]
    [ChildType(typeof(PiffHintBytesSent64Box))]
    [ChildType(typeof(PiffHintMaxRateBox))]
    [ChildType(typeof(PiffHintMediaBytesSentBox))]
    [ChildType(typeof(PiffHintImmediateBytesSentBox))]
    [ChildType(typeof(PiffHintRepeatedBytesSentBox))]
    [ChildType(typeof(PiffHintMinRelativeTimeBox))]
    [ChildType(typeof(PiffHintMaxRelativeTimeBox))]
    [ChildType(typeof(PiffHintLargestPacketBox))]
    [ChildType(typeof(PiffHintLongestPacketBox))]
    [ChildType(typeof(PiffHintPayloadIdBox))]
    public sealed class PiffHintStatisticsBox : PiffBoxBase
    {
    }
}
