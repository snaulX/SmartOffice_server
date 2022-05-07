using System;

namespace Fbay
{
    /// <summary>
    /// Manager of debug logs, errors or warnings
    /// </summary>
    public static class Debug
    {
        public static event Action<object> OnLog, OnError, OnWarning;

        public static void Log(object message) => OnLog?.Invoke(message);
        public static void Error(object message) => OnError?.Invoke(message);
        public static void Warning(object message) => OnWarning?.Invoke(message);
    }
}
