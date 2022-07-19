using System;
using System.Linq;
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
        /// The box with this length value extends to the end of the input stream.
        /// </summary>
        public const ulong AutoExtend = 0;


        /// <summary>
        /// If the length needs to be 64 bits, it follows the standard header.
        /// Then the standard header length is set to this value.
        /// </summary>
        public const ulong Length64 = 1;

        #endregion


        #region Inherited properties


        /// <summary>
        /// The actual box name - useful for boxes with multiple names.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Skip)]
        public string BoxType { get; set; }


        /// <summary>
        /// Box'es position when read from a file (the first byte of the length).
        /// It might not be the same during writing.
        /// </summary>
        [PiffDataFormat(PiffDataFormats.Skip)]
        public long Position { get; set; }


        /// <summary>
        /// Children boxes.
        /// All boxes have their children as the only fields,
        /// or as the last fields.
        /// </summary>
        [AfterDescendants]
        public PiffBoxBase[] Children { get; set; }

        #endregion


        #region Init and clean-up

        public PiffBoxBase()
        {
            // When creating boxes in code, use the first name by default
            BoxType = GetType().GetCustomAttribute<BoxNameAttribute>().Name;
        }

        #endregion


        #region Virtual API

        /// <summary>
        /// Used for logging / debugging.
        /// </summary>
        public override string ToString() => BoxType;

        #endregion


        #region API

        /// <summary>
        /// Return the first child of the given type.
        /// There may be more of those, they are ignored.
        /// </summary>
        public TBox FirstOfType<TBox>() where TBox : PiffBoxBase
        {
            return Children?.OfType<TBox>().FirstOrDefault();
        }


        /// <summary>
        /// Return the first grandchild for the given path.
        /// There may be more children on every junction, they are ignored.
        /// </summary>
        public TBoxL2 FirstOfType<TBoxL1, TBoxL2>()
            where TBoxL1 : PiffBoxBase
            where TBoxL2 : PiffBoxBase
        {
            return FirstOfType<TBoxL1>()?.Children.OfType<TBoxL2>().FirstOrDefault();
        }


        /// <summary>
        /// Return the first great-grandchild for the given path.
        /// There may be more children on every junction, they are ignored.
        /// </summary>
        public TBoxL3 FirstOfType<TBoxL1, TBoxL2, TBoxL3>()
            where TBoxL1 : PiffBoxBase
            where TBoxL2 : PiffBoxBase
            where TBoxL3 : PiffBoxBase
        {
            return FirstOfType<TBoxL1, TBoxL2>()?.Children.OfType<TBoxL3>().FirstOrDefault();
        }


        /// <summary>
        /// Return the first great-grandchild for the given path.
        /// There may be more children on every junction, they are ignored.
        /// </summary>
        public TBoxL4 FirstOfType<TBoxL1, TBoxL2, TBoxL3, TBoxL4>()
            where TBoxL1 : PiffBoxBase
            where TBoxL2 : PiffBoxBase
            where TBoxL3 : PiffBoxBase
            where TBoxL4 : PiffBoxBase
        {
            return FirstOfType<TBoxL1, TBoxL2, TBoxL3>()?.Children.OfType<TBoxL4>().FirstOrDefault();
        }


        /// <summary>
        /// Return all children of the given type.
        /// </summary>
        public TBox[] ChildrenOfType<TBox>() where TBox : PiffBoxBase
        {
            if (Children is null)
                return default;

            return Children.OfType<TBox>().ToArray();
        }


        #endregion
    }
}
