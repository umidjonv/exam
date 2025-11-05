using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

namespace Win32Pinvoke
{
    /// <summary>
    /// https://www.pinvoke.net/default.aspx/winmm.waveoutgetvolume
    /// </summary>
    public static class WinMm
    {

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern uint waveOutGetNumDevs();
         
        public static IDictionary<string, string> GetAudioOutDevices()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice");
            var collection = searcher.Get();
            var dic = new Dictionary<string, string>();

            foreach (var item in collection)
            {
                foreach (var property in item.Properties)
                {
                    dic.Add(property.Name, (string)property.Value);
                }
            }

            return dic;
        }

    }
}