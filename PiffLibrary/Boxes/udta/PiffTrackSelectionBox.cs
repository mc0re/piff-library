namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Marks tracks that belong to the same switch group
    /// (available for switching during playback,
    /// as opposite to alternate group, available for selection before the playback).
    /// 
    /// A switch group is a subset of an alternate group.
    /// </summary>
    [BoxName("tsel")]
    public sealed class PiffTrackSelectionBox : PiffFullBoxBase
    {
        /// <summary>
        /// Group number or 0 (no information on switching).
        /// </summary>
        public int SwitchGroup { get; set; }


        /// <summary>
        /// Differentiation criteria for tracks in the same group.
        /// 
        /// Values determining track characteristics:
        /// - tesc - temporal scalability
        /// - fgsc - fine grain SNR quality scalability
        /// - cgsc - coarse grain SNR quality scalability
        /// - spsc - spatial scalability
        /// - resc - region-of-interest scalability
        /// - vwsc - number of views scalability
        /// 
        /// Values differentiating the tracks:
        /// - cdec - codec, see <see cref="PiffSampleDescriptionBox"/>
        /// - scsz - width/height fields of the visual samples
        /// - mpsz - max packet size in RTP hint sample entry
        /// - mtyp - media type, see <see cref="PiffHandlerTypeBox.HandlerType"/>
        /// - mela - media language, see <see cref="PiffMediaHeaderBox.Language"/>
        /// - bitr - average bitrate of the track (total sample size / duration)
        /// - frar - frame rate
        /// - nvws - number of views in the sub track
        /// </summary>
        [PiffStringLength(4)]
        public string[] Attributes { get; set; }
    }
}
