using System;
using System.Collections.Generic;


namespace PiffLibrary
{
    internal sealed class PiffReadContext
    {
        /// <summary>
        /// Reading hierarchy. Must be empty when the file is read out.
        /// </summary>
        public Stack<PiffBoxBase> Hierarchy { get; } = new Stack<PiffBoxBase>();


        /// <summary>
        /// A list of read boxes.
        /// </summary>
        public IList<string> Dump { get; } = new List<string>();


        /// <summary>
        /// A list of parsing warnings.
        /// </summary>
        public IList<string> Messages { get; } = new List<string>();


        /// <summary>
        /// Used for debugging.
        /// </summary>
        public string CurrentBoxName => Hierarchy.Peek()?.ToString() ?? "top";


        /// <summary>
        /// Whether this is a known and expected child type.
        /// </summary>
        internal bool IsExpectedBoxType(Type type) => true;


        /// <summary>
        /// Add a new warning during reading.
        /// </summary>
        internal void AddWarning(string message) => Messages.Add(message);


        internal void AddError(string message) => Messages.Add(message);


        internal void Push(PiffBoxBase box)
        {
            Dump.Add(new string(' ', Hierarchy.Count * 2) + box.ToString());
            Hierarchy.Push(box);
        }


        internal void Pop()
        {
            Hierarchy.Pop();
        }
    }
}
