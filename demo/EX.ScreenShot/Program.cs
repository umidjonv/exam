using System;
using System.Drawing.Imaging;
using System.IO;
using EX.Common.Helpers;
using Win32Pinvoke;

namespace EX.ScreenShot
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var file = Path.Combine(Path.GetTempPath(), $"{DateTime.Now:yyyyddMMHHmmss}.jpg");

            User32.ScreenCapture(file, ImageFormat.Jpeg);

            ProcessHelper.OpenFile(file);

            Console.WriteLine("Done!", args);
        }
    }
}