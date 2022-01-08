using System;
using System.Collections.Generic;

namespace PiffLibrary
{
    public class PiffWriteContext
    {
        private int mLevel;

        public IList<string> Dump { get; } = new List<string>();


        internal void Start(PiffBoxBase obj)
        {
            Dump.Add(new string(' ', mLevel * 2) + obj.ToString());
            mLevel++;
        }


        internal void End(PiffBoxBase _)
        {
            mLevel--;
        }
    }
}