using System.Drawing;
using System.Windows.Forms;

namespace EasyToast
{

    /// <summary>
    /// Make your own custom Toast Notification by the Builder
    /// </summary>
    public static class ToastBuilder
	{
		/// <summary>
		/// Create an empty Toast
		/// </summary>
		/// <param name="window"></param>
		/// <returns></returns>
		public static Toast Create(IWin32Window window)
		{
			return new Toast(window);
		}

		/// <summary>
		/// Set text for toast
		/// </summary>
		/// <param name="toast">toast</param>
		/// <param name="text">Text data to display</param>
		/// <returns></returns>
		public static Toast SetTitle(this Toast toast, string text)
		{
			toast.Title = text;

			return toast;
		}

		/// <summary>
		/// Set text for toast
		/// </summary>
		/// <param name="toast">toast</param>
		/// <param name="description">Text data to display</param>
		/// <returns></returns>
		public static Toast SetDescription(this Toast toast, string description)
		{
			toast.Description = description;

			return toast;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toast"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static Toast SetDuration(this Toast toast, Duration duration)
		{
			toast.Duration = duration;
		
			return toast;
		}

		public static Toast SetMuting(this Toast toast, bool muting)
		{
			toast.IsMuted = muting;
			
			return toast;
		}

		public static Toast SetThumbnail(this Toast toast, Image image)
		{
			toast.Thumbnail = image;

			return toast;
		}
	}

}
