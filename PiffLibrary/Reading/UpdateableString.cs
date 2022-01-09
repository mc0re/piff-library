using System;


namespace PiffLibrary
{
    internal class UpdateableString
    {
        private readonly object mSource;
        private readonly Func<object, object[], string> mGeneratorFn;
        private readonly object[] mStatics;


        public UpdateableString(object source, Func<object, object[], string> generatorFn, params object[] statics)
        {
            mSource = source;
            mGeneratorFn = generatorFn;
            mStatics = statics;
        }


        public override string ToString()
        {
            return mGeneratorFn.Invoke(mSource, mStatics);
        }
    }
}