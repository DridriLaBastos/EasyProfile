using System;
namespace EasyProfile
{
    public class Profiler
    {
        static private DateTime last = DateTime.Now;

        private static int dynamicTimeInterval = 1;
        public static int DynamicTimeInterval { get => dynamicTimeInterval; set { if (value <= 0) { throw new Exception($"Time interval between statistics display of the profiler cannot be <= 0 (you choose {value})"); } dynamicTimeInterval = value; } }

        public static ProfilerNode Profile(in string profileContextName)
        {
            return ProfilerNode.CurrentContext.GetChild(profileContextName).Start();
        }

        public static ProfilerNode DynamicProfile(in string profileContextName)
        {
            DateTime now = DateTime.Now;
            if ((now - last).Seconds >= dynamicTimeInterval)
            {
                last = now;
                ProfilerContext.staticContext.PrintResults();
            }

            return Profile(profileContextName);
        }

        public static void Start(in string profilerContextName)
        { ProfilerNode.CurrentContext.GetChild(profilerContextName).Start(); }

        public static void DynamicStart(in string profilerContextName)
        {
            DateTime now = DateTime.Now;
            if ((now - last).Seconds >= dynamicTimeInterval)
            {
                last = now;
                ProfilerContext.staticContext.PrintResults();
            }

            Start(profilerContextName);
        }

        public static void Stop()
        { ProfilerNode.CurrentContext.Dispose(); }

        public static void PrintResults()
        {
            ProfilerContext.staticContext.PrintResults();
        }
    }
}