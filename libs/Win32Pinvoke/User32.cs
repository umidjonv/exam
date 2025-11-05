using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Win32Pinvoke
{
    public static class User32
    {
        /// <summary>
        /// Gets the handle of the window that currently has focus.
        /// </summary>
        /// <returns>
        /// The handle of the window that currently has focus.
        /// </returns>
        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Activates the specified window.
        /// </summary>
        /// <param name="hWnd">
        /// The handle of the window to be focused.
        /// </param>
        /// <returns>
        /// True if the window was focused; False otherwise.
        /// </returns>
        [DllImport("user32")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Windows API function to animate a window.
        /// </summary>
        [DllImport("user32")]
        public extern static bool AnimateWindow(IntPtr hWnd, int dwTime, int dwFlags);

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public struct ScreenSize
        {
            public int screenLeft;
            public int screenTop;
            public int screenRight;
            public int screenBottom;
        }

        private const int ENUM_CURRENT_SETTINGS = -1;

        private const int ENUM_REGISTRY_SETTINGS = -2;

        [Flags]
        private enum DisplayDeviceStateFlags : int
        {

            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,

            /// <summary>This is the primary display.</summary>
            PrimaryDevice = 0x4,

            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,

            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x16,

            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,

            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct DisplayDevice
        {

            [MarshalAs(UnmanagedType.U4)]
            public int cb;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;

            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;

        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DEVMODE
        {

            private const int CCHDEVICENAME = 0x20;

            private const int CCHFORMNAME = 0x20;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;

            public short dmSpecVersion;

            public short dmDriverVersion;

            public short dmSize;

            public short dmDriverExtra;

            public int dmFields;

            public int dmPositionX;

            public int dmPositionY;

            public ScreenOrientation dmDisplayOrientation;

            public int dmDisplayFixedOutput;

            public short dmColor;

            public short dmDuplex;

            public short dmYResolution;

            public short dmTTOption;

            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;

            public short dmLogPixels;

            public int dmBitsPerPel;

            public int dmPelsWidth;

            public int dmPelsHeight;

            public int dmDisplayFlags;

            public int dmDisplayFrequency;

            public int dmICMMethod;

            public int dmICMIntent;

            public int dmMediaType;

            public int dmDitherType;

            public int dmReserved1;

            public int dmReserved2;

            public int dmPanningWidth;

            public int dmPanningHeight;

        }

        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        private static extern int EnumDisplayDevices(string lpDevice, int iDevNum, ref DisplayDevice lpDisplayDevice, int dwFlags);

        public static ScreenSize GetScreen()
        {
            // Initialize the virtual screen to dummy values
            var screenLeft = 0;
            var screenTop = 0;
            var screenRight = 0;
            var screenBottom = 0;

            // Enumerate system display devices
            var deviceIndex = 0;

            while (true)
            {
                var deviceData = new DisplayDevice
                {
                    cb = Marshal.SizeOf(typeof(DisplayDevice))
                };

                if (EnumDisplayDevices(null, deviceIndex, ref deviceData, 0) == 0)
                {
                    break;
                }
                else
                {
                    // Get the position and size of this particular display device
                    var devMode = new DEVMODE();

                    if (EnumDisplaySettings(deviceData.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        // Update the virtual screen dimensions
                        screenLeft = Math.Min(screenLeft, devMode.dmPositionX);
                        screenTop = Math.Min(screenTop, devMode.dmPositionY);
                        screenRight = Math.Max(screenRight, devMode.dmPositionX + devMode.dmPelsWidth);
                        screenBottom = Math.Max(screenBottom, devMode.dmPositionY + devMode.dmPelsHeight);
                    }

                    deviceIndex++;
                }
            }

            // Create a bitmap of the appropriate size to receive the screen-shot.
            return new ScreenSize
            {
                screenRight = screenRight,
                screenLeft = screenLeft,
                screenBottom = screenBottom,
                screenTop = screenTop
            };
        }

        public static void ScreenCapture(string filename, ImageFormat format)
        {
            // Initialize the virtual screen to dummy values
            var screenLeft = 0;
            var screenTop = 0;
            var screenRight = 0;
            var screenBottom = 0;

            // Enumerate system display devices
            var deviceIndex = 0;

            while (true)
            {
                var deviceData = new DisplayDevice
                {
                    cb = Marshal.SizeOf(typeof(DisplayDevice))
                };

                if (EnumDisplayDevices(null, deviceIndex, ref deviceData, 0) == 0)
                {
                    break;
                }
                else
                {
                    // Get the position and size of this particular display device
                    var devMode = new DEVMODE();

                    if (EnumDisplaySettings(deviceData.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        // Update the virtual screen dimensions
                        screenLeft = Math.Min(screenLeft, devMode.dmPositionX);
                        screenTop = Math.Min(screenTop, devMode.dmPositionY);
                        screenRight = Math.Max(screenRight, devMode.dmPositionX + devMode.dmPelsWidth);
                        screenBottom = Math.Max(screenBottom, devMode.dmPositionY + devMode.dmPelsHeight);
                    }

                    deviceIndex++;
                }
            }

            // Create a bitmap of the appropriate size to receive the screen-shot.
            using var bmp = new Bitmap(screenRight - screenLeft, screenBottom - screenTop);
            // Draw the screen-shot into our bitmap.
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
            }

            // Stuff the bitmap into a file
            bmp.Save(filename, format);
        }

        public static void ScreenCapture(Stream stream, ImageFormat format)
        {
            // Initialize the virtual screen to dummy values
            var screenLeft = 0;
            var screenTop = 0;
            var screenRight = 0;
            var screenBottom = 0;

            // Enumerate system display devices
            var deviceIndex = 0;

            while (true)
            {
                var deviceData = new DisplayDevice
                {
                    cb = Marshal.SizeOf(typeof(DisplayDevice))
                };

                if (EnumDisplayDevices(null, deviceIndex, ref deviceData, 0) == 0)
                {
                    break;
                }
                else
                {
                    // Get the position and size of this particular display device
                    var devMode = new DEVMODE();

                    if (EnumDisplaySettings(deviceData.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        // Update the virtual screen dimensions
                        screenLeft = Math.Min(screenLeft, devMode.dmPositionX);
                        screenTop = Math.Min(screenTop, devMode.dmPositionY);
                        screenRight = Math.Max(screenRight, devMode.dmPositionX + devMode.dmPelsWidth);
                        screenBottom = Math.Max(screenBottom, devMode.dmPositionY + devMode.dmPelsHeight);
                    }

                    deviceIndex++;
                }
            }

            // Create a bitmap of the appropriate size to receive the screen-shot.
            using var bmp = new Bitmap(screenRight - screenLeft, screenBottom - screenTop);

            // Draw the screen-shot into our bitmap.
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
            }

            // Stuff the bitmap into a file
            bmp.Save(stream, format);
        }
    }
}