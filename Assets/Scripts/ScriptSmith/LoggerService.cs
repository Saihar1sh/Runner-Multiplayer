using System;

namespace Arixen.ScriptSmith
{
    public static class LoggerService
    {
        public enum LogLevel
        {
            Log,
            LowWarning,
            HighWarning,
            Error,
        }

        public static void Debug(string message, LogLevel level = LogLevel.Log)
        {
            switch (level)
            {
                case LogLevel.Log:
                    UnityEngine.Debug.Log("<color=green> "+message+"</color>");
                    break;
                case LogLevel.LowWarning:
                    UnityEngine.Debug.LogWarning("<color=#FFAA33> "+message+"</color>");
                    break;
                case LogLevel.HighWarning:
                    UnityEngine.Debug.LogWarning("<color=#FFA500> "+message+"</color>");
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError("<color=red> "+message+"</color>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}