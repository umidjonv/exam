using System.Drawing;

namespace EasyToast
{
    public class ColorScheme
	{
		private byte RBg { get; set; }
		private byte BBg { get; set; }
		private byte GBg { get; set; }
		private byte RFg { get; set; }
		private byte BFg { get; set; }
		private byte GFg { get; set; }

		/// <summary>
		/// Create new color scheme
		/// </summary>
		/// <param name="rbg"></param>
		/// <param name="bbg"></param>
		/// <param name="gbg"></param>
		/// <param name="rfg"></param>
		/// <param name="bfg"></param>
		/// <param name="gfg"></param>
		public ColorScheme(byte rbg, byte bbg, byte gbg, byte rfg, byte bfg, byte gfg)
		{
			RBg = rbg;
			BBg = bbg;
			GBg = gbg;
			RFg = rfg;
			BFg = bfg;
			GFg = gfg;
		}

		public Color GetBackgroundColor()
		{
			return Color.FromArgb(RBg, BBg, GBg);
		}

		public Color GetForegroundColor()
		{
			return Color.FromArgb(RFg, BFg, GFg);
		}
	}

}
