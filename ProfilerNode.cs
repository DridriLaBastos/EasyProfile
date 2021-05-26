using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace EasyProfile
{
    public class ProfilerNode : IDisposable
    {
        protected Stopwatch mWatch;
        protected ProfilerNode mParent;
        protected List<ProfilerNode> mChilds;
        protected int mChildIndex;
        protected int mCallCount;
        protected bool mLooped;

        public string mName { get; private set; }

        public static ProfilerNode CurrentContext { get; protected set; }

        public ProfilerNode(in string name, in ProfilerNode parent)
        {
            mChildIndex = 0;
            mName = name;
            mParent = parent;
            mChilds = new List<ProfilerNode>(2);
            mWatch = new Stopwatch();
            mLooped = false;
        }

        public ProfilerNode Start()
        {
            ++mCallCount;
            mWatch.Start();
            return this;
        }

        public ProfilerNode GetChild(in string childName)
        {
            if (!mLooped)
            {
                if (mChilds.Count > 0 && mChilds[0].mName == childName)
                {
                    mChildIndex = 0;
                    mLooped = true;
                }
                else
                {
                    mChilds.Add(new ProfilerNode(childName, this));
                    mChildIndex = mChilds.Count - 1;
                }
            }

            int index = mChildIndex;
            mChildIndex = (mChildIndex + 1) % mChilds.Count;
            CurrentContext = mChilds[index];

            return CurrentContext;
        }

        public void Dispose()
        {
            mWatch.Stop();
            CurrentContext = mParent;
        }

        public void PrintResults(int level)
        {
            for (int i = 0; i < level; i++)
                Console.Write("    ");

            Console.Write($"{mName}: {mCallCount} in {mWatch.ElapsedMilliseconds}ms ({(float)mWatch.ElapsedMilliseconds / mCallCount} ms/calls");

            if (level != 0)
                Console.Write($" - { (float)mWatch.ElapsedTicks / mParent.mWatch.ElapsedTicks * 100f }%");

            Console.WriteLine(')');

            foreach (ProfilerNode child in mChilds)
                child.PrintResults(level + 1);

            mWatch.Reset();
            mCallCount = 0;
        }
    }
}