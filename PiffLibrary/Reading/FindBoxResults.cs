using PiffLibrary.Boxes;


namespace PiffLibrary.Infrastructure
{
    internal enum FindBoxResults
    {
        /// <summary>
        /// The box ID is found and is correctly located.
        /// </summary>
        Found,

        /// <summary>
        /// The box ID is found, but its location (parent) is unexpected.
        /// Thst is, it is not mentioned in <see cref="ChildTypeAttribute"/>
        /// of the parent type.
        /// </summary>
        Unexpected,


        /// <summary>
        /// More than one child has the same box name.
        /// </summary>
        Ambiguous,


        /// <summary>
        /// The box ID is not recognized.
        /// <see cref="PiffCatchAllBox"/> is returned in this case.
        /// </summary>
        Unrecognized
    }
}
