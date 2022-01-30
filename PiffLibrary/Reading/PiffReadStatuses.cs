namespace PiffLibrary
{
    public enum PiffReadStatuses
    {
        /// <summary>
        /// For reading utility methods, this return code means "all ok for now".
        /// </summary>
        Continue,


        /// <summary>
        /// There was an unrecoverable error during reading.
        /// Skip to the end of the container box.
        /// </summary>
        SkipToEnd,


        /// <summary>
        /// End of file reached when trying to read a new box.
        /// File reading is finished successfully.
        /// </summary>
        Eof,


        /// <summary>
        /// End of file reached while expecting data.
        /// </summary>
        EofPremature
    }
}
