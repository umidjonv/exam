using System.Runtime.InteropServices;

namespace Win32Pinvoke
{
    public static class Kernel32
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GenerateConsoleCtrlEvent(ConsoleCtrlEvent sigevent, int dwProcessGroupId);

        internal enum ConsoleCtrlEvent
        {
            CTRL_C = 0,
            CTRL_BREAK = 1,
            CTRL_CLOSE = 2,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN = 6
        }

        public static void ExecuteCtrlC(int id)
        {
            GenerateConsoleCtrlEvent(ConsoleCtrlEvent.CTRL_C, id);
        }

    }
}