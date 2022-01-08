﻿using System.Linq;
using System.Reflection;


namespace PiffLibrary
{
    /// <summary>
    /// Base class for all boxes with some common functionality.
    /// </summary>
    internal abstract class PiffBoxBase
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
        public PiffBoxBase[] Childen { get; set; } = new PiffBoxBase[0];

        #endregion


        #region Virtual API

        /// <summary>
        /// Used for logging / debugging.
        /// </summary>
        public override string ToString() => GetType().GetCustomAttribute<BoxNameAttribute>().Name;

        #endregion
    }
}
