using System.Runtime.InteropServices;

namespace Win32Pinvoke
{
    public static class WinInet
    {

        /// <summary>
        /// http://pinvoke.net/default.aspx/wininet/InternetGetConnectedState.html
        /// </summary>
        /// <param name="description"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool InternetIsConnected()
        {
            return InternetGetConnectedState(out _, 0);
        }

    }
}
