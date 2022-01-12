using PiffLibrary.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PiffLibrary
{
    internal sealed class PiffReadContext
    {
        #region Properties

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
        /// Used for debugging.
        /// </summary>
        public string CurrentBoxName => Hierarchy.Any() ? Hierarchy.Peek().ToString() : "- None -";

        #endregion


        /// <summary>
        /// Whether this is a known and expected child type.
        /// </summary>
        internal bool IsExpectedBoxType(Type type) => true;


        /// <summary>
        /// Add a new warning during reading.
        /// </summary>
        internal void AddWarning(string message) => Messages.Add(message);


        internal void AddError(string message) => Messages.Add(message);


        internal void Push(PiffBoxBase box, long position)
        {
            Dump.Add(new UpdateableString(
                box, (o, st) => $"{st[0]}{o} (:{st[1]})",
                new string(' ', Hierarchy.Count * 2), position));
            Hierarchy.Push(box);
        }


        internal void Pop()
        {
            Hierarchy.Pop();
        }
    }
}
