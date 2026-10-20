using PiffLibrary.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PiffLibrary
{
    public sealed class PiffReadContext
    {
        #region Properties

        /// <summary>
        /// Ignore box type check on the root level.
        /// </summary>
        public bool AnyRoot { get; set; }


        /// <summary>
        /// Seek over the payload of media data boxes ("mdat") instead of reading it.
        /// The boxes are kept, with <see cref="PiffMediaDataBox.RawData"/> set to <see langword="null"/>.
        /// Allows reading the structure of large files without loading the media.
        /// On a stream that cannot seek the payload is read and discarded instead.
        /// </summary>
        public bool SkipMediaData { get; set; }


        /// <summary>
        /// Reading hierarchy. Must be empty when the file is read out.
        /// </summary>
        public Stack<PiffBoxBase> Hierarchy { get; } = new Stack<PiffBoxBase>();


        /// <summary>
        /// A list of read boxes.
        /// </summary>
        public IList<UpdateableString> Dump { get; } = new List<UpdateableString>();


        /// <summary>
        /// A list of parsing warnings.
        /// </summary>
        public IList<string> Messages { get; } = new List<string>();


        /// <summary>
        /// If there were any error messages.
        /// </summary>
        public bool IsError { get; private set; }


        /// <summary>
        /// Used for checking children.
        /// </summary>
        public PiffBoxBase CurrentBox => Hierarchy.Any() ? Hierarchy.Peek() : null;


        /// <summary>
        /// Used for debugging.
        /// </summary>
        public string CurrentBoxName => Hierarchy.Any() ? Hierarchy.Peek().ToString() : "- None -";

        #endregion


        /// <summary>
        /// Whether this is a known and expected child type.
        /// </summary>
        internal bool IsExpectedBoxType(Type type) => true;


        /// <summary>
        /// Add a new information message during reading.
        /// </summary>
        internal void AddInfo(string message) => Messages.Add(message);


        /// <summary>
        /// Add a new warning during reading.
        /// </summary>
        internal void AddWarning(string message) => Messages.Add(message);


        /// <summary>
        /// Add a new error message during reading.
        /// </summary>
        internal void AddError(string message)
        {
            IsError = true;
            Messages.Add(message);
        }


        internal void Push(PiffBoxBase box, ulong position, ulong size)
        {
#if DEBUG
            Dump.Add(new UpdateableString(
                box, (o, st) => $"{st[0]}{o} (:{st[1]} + {st[2]})",
                new string(' ', Hierarchy.Count * 2), position, size));
#endif

            Hierarchy.Push(box);
        }


        internal void Pop()
        {
            Hierarchy.Pop();
        }
    }
}
