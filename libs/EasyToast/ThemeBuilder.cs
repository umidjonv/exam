using System.Drawing;

namespace EasyToast
{
    public class ThemeBuilder
	{
		internal static ColorScheme CustomScheme;

		public static void CreateCustomScheme(Color backgroundColor, Color foregroundColor)
		{
			CustomScheme = new ColorScheme(backgroundColor.R, backgroundColor.B, backgroundColor.G, foregroundColor.R, foregroundColor.B, foregroundColor.G);
		}

		internal static class BuiltinScheme
		{
			internal static readonly ColorScheme DarkScheme = new ColorScheme(33, 33, 33, 255,255,255);
			internal static readonly ColorScheme LightScheme = new ColorScheme(255, 255, 255, 33, 33, 33);
			internal static readonly ColorScheme PrimaryLightScheme = new ColorScheme(33, 150, 243, 255,255,255);
			internal static readonly ColorScheme SuccessLightScheme = new ColorScheme(76, 175, 80, 255, 255, 255);
			internal static readonly ColorScheme WarningLightScheme = new ColorScheme(255, 152, 0, 255, 255, 255);
			internal static readonly ColorScheme ErrorLightScheme = new ColorScheme(213, 0, 0, 255, 255, 255);
			internal static readonly ColorScheme PrimaryDarkScheme = new ColorScheme(33, 33, 33, 33, 150, 243);
			internal static readonly ColorScheme SuccessDarkScheme = new ColorScheme(33, 33, 33, 76, 175, 80);
			internal static readonly ColorScheme WarningDarkScheme = new ColorScheme(33, 33, 33, 255, 152, 0);
			internal static readonly ColorScheme ErrorDarkScheme = new ColorScheme(33, 33, 33, 213,0,0);
		}
	}

}
