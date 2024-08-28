using System.Diagnostics;

namespace AICoderVS
{
    internal static class MyLog
    {
        public static void Log(string message)
        {
            Debugger.Log(0, null, message + '\n');
        }
    }
}