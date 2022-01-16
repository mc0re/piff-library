namespace PiffLibrary.Boxes
{
    [BoxName("stri")]
    public sealed class PiffSubTrackInformationBox : PiffFullBoxBase
    {
        /// <summary>
        /// Group number or 0 (no information on switching).
        /// </summary>
        public short SwitchGroup { get; set; }


        /// <summary>
        /// Group number or 0 (no information on alternation).
        /// </summary>
        public short AlternateGroup { get; set; }


        /// <summary>
        /// Sub-group ID or 0 (not assigned).
        /// </summary>
        public uint SubTrackId { get; set; }


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
        /// - bitr - average bitrate of the track (total sample size / duration)
        /// - frar - frame rate
        /// - nvws - number of views in the sub track
        /// </summary>
        /// <remarks>See <see cref="PiffTrackSelectionBox.Attributes"/>.</remarks>
        [PiffStringLength(4)]
        public string[] Attributes { get; set; }
    }
}
