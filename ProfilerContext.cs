using System;

namespace EasyProfile
{
    class ProfilerContext: ProfilerNode
    {
        private ProfilerContext(): base("root",null)
        { CurrentContext = this; }

        public void PrintResults()
        {
            foreach (ProfilerNode child in mChilds)
                child.PrintResults(0);
        }

        public static readonly ProfilerContext staticContext = new ProfilerContext();
    }
}
