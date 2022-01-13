using System.Reflection;


namespace PiffLibrary.Boxes
{
    /// <summary>
    /// Base class for all boxes with some common functionality.
    /// </summary>
    public abstract class PiffBoxBase
    {
        #region Constants

        /// <summary>
        /// Size of the box type [bytes].
        /// </summary>
        public const int BoxTypeLength = 4;


        /// <summary>
        /// Length field and box type [bytes].
        /// </summary>
        public const uint HeaderLength = sizeof(int) + BoxTypeLength;


        /// <summary>
        /// If the length needs to be 64 bits, it follows the standard header.
        /// Then the standard header length is set to this value.
        /// </summary>
        public const uint Length64 = 1;

        #endregion


        #region Inherited properties

        /// <summary>
        /// Children boxes.
        /// All boxes have their children as the only fields,
        /// or as the last fields.
        /// </summary>
        [AfterDescendants]
        public PiffBoxBase[] Childen { get; set; }

        #endregion


        #region Virtual API

        /// <summary>
        /// Used for logging / debugging.
        /// </summary>
        public override string ToString() => GetType().GetCustomAttribute<BoxNameAttribute>().Name;

        #endregion


        #region API

        /// <summary>
        /// Return the first child of the given type.
        /// </summary>
        public TBox First<TBox>() where TBox : PiffBoxBase
        {
            if (Childen is null) return default;

            foreach (var child in Childen)
            {
                if (child is TBox box)
                    return box;
            }

            return default;
        }

        #endregion
    }
}
