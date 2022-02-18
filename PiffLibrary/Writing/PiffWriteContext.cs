using System.Collections.Generic;
using PiffLibrary.Boxes;


namespace PiffLibrary
{
    public class PiffWriteContext
    {
        private int mLevel;

        public IList<string> Dump { get; } = new List<string>();


        public IList<string> Errors { get; } = new List<string>();


        internal void Start(PiffBoxBase box, long position)
        {
            Dump.Add(new string(' ', mLevel * 2) + $"{box} (:{position})");
            mLevel++;
        }


        internal void End(PiffBoxBase _)
        {
            mLevel--;
        }


        public void AddError(string message)
        {
            Errors.Add(message);
        }
    }
}