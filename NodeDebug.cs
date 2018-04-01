using System;

namespace MyNamespace.Utils
{
    public static class NodeDebug
    {
        public class DebugModuleTag
        {
            public const int RUNTIME = 1 << 0;
            public const int EDITOR = 1 << 1;
            public const int VALUE_HOLDER = 1 << 2;
            public const int GRPAH_LIFECYCLE = 1 << 3;
        }

        public enum DebugLevel
        {
            Log = 0,
            Warning,
            Error,
            None
        }

        public static Action<string> logCallback;
        public static Action<string> logWarningCallback;
        public static Action<string> logErrorCallback;

        public static DebugLevel debugLevel = DebugLevel.Log;
        public static int moduleDebugSwitch = DebugModuleTag.GRPAH_LIFECYCLE;

        public static void Log(object content)
        {
            Log(content, moduleDebugSwitch);
        }

        public static void Log(object content, int tag)
        {
            if (debugLevel > DebugLevel.Log)
                return;

            if (logCallback == null)
                return;

            if ((moduleDebugSwitch & tag) == 0)
                return;

            logCallback(content + "");
        }

        public static void LogWarning(object content)
        {
            LogWarning(content, moduleDebugSwitch);
        }

        public static void LogWarning(object content, int tag)
        {
            if (debugLevel > DebugLevel.Warning)
                return;

            if (logWarningCallback == null)
                return;

            if ((moduleDebugSwitch & tag) == 0)
                return;

            logWarningCallback(content + "");
        }

        public static void LogError(object content)
        {
            LogError(content, moduleDebugSwitch);
        }

        public static void LogError(object content, int tag)
        {
            if (debugLevel > DebugLevel.Error)
                return;

            if (logErrorCallback == null)
                return;

            if ((moduleDebugSwitch & tag) == 0)
                return;

            logErrorCallback(content + "");
        }
    }
}
