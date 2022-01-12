using PiffLibrary.Boxes;
using System.Collections.Generic;

namespace PiffLibrary
{
    public class PiffWriteContext
    {
        private int mLevel;

        public IList<string> Dump { get; } = new List<string>();


        internal void Start(PiffBoxBase box, long position)
        {
            Dump.Add(new string(' ', mLevel * 2) + $"{box} (:{position})");
            mLevel++;
        }


        internal void End(PiffBoxBase _)
        {
            mLevel--;
        }
    }
}