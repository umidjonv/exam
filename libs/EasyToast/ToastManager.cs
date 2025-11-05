using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EasyToast
{
    /// <summary>
    /// Use the Manager to managing multiple Toast widgets
    /// </summary>
    public static class ToastManager
    {
        public const byte MAX_TOASTS_ALLOWED = 10;
        internal static Toast Toast;

        /// <summary>
        /// Get all toasts displaying
        /// </summary>
        public static ToastCollection ToastCollection { get; } = new ToastCollection();

        /// <summary>
        /// Add toast to collection
        /// </summary>
        internal static void AddToCollection()
        {
            if (ToastCollection.Count >= MAX_TOASTS_ALLOWED) return;
          
            if (string.IsNullOrWhiteSpace(Toast.Title))
            {
                throw new ArgumentException("Text property is required to display Toast");
            }

            Toast.FrmToast.CloseStyle = Toast.CloseStyle;
            Toast.FrmToast.IsMuted = Toast.IsMuted;
            Toast.FrmToast.Toast = Toast;
            Toast.FrmToast.Duration = Toast.Duration;
            Toast.FrmToast.Animation = Toast.Animation;
            Toast.FrmToast.Title = Toast.Title;
            Toast.FrmToast.Description = Toast.Description;
            Toast.FrmToast.Thumbnails = Toast.Thumbnail;
            Toast.FrmToast.Theme = Toast.ThemeStyle;

            SetLocation(Toast.Position);

            ToastCollection.Add(Toast);
            Toast.FrmToast.Show(Toast.Window);

        }

        private static void SetLocation(Position position)
        {
            switch (position)
            {
                case Position.TopRight:
                    var rightmost = Screen.AllScreens[0];
                    foreach (var screen in Screen.AllScreens)
                    {
                        if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                            rightmost = screen;
                    }

                    if (ToastCollection.Count == 0)
                    {
                        Toast.FrmToast.Left = rightmost.WorkingArea.Right - Toast.FrmToast.Width - Toast.GetHorizontalMargin();
                        Toast.FrmToast.Top = rightmost.WorkingArea.Top + Toast.GetVerticalMargin();
                    }
                    else
                    {
                        var collection = ToastCollection.GetTopRightToasts();
                        var enumerable = collection as List<Toast> ?? collection.ToList();
                        if (enumerable.Count == 0)
                        {
                            Toast.FrmToast.Left = rightmost.WorkingArea.Right - Toast.FrmToast.Width - Toast.GetHorizontalMargin();
                            Toast.FrmToast.Top = rightmost.WorkingArea.Top + Toast.GetVerticalMargin();
                        }
                        else if (enumerable.Count < 3)
                        {
                            Toast.FrmToast.Left = rightmost.WorkingArea.Right - Toast.FrmToast.Width - Toast.GetHorizontalMargin();
                            Toast.FrmToast.Top = rightmost.WorkingArea.Top + enumerable.Count * Toast.FrmToast.Height + enumerable.Count * Toast.GetVerticalMargin() + Toast.GetVerticalMargin();
                        }
                    }

                    break;
                case Position.BottomRight:
                    {
                        var workingArea = Screen.GetWorkingArea(Toast.FrmToast);
                        if (ToastCollection.Count == 0)
                        {
                            Toast.FrmToast.Location = new Point(workingArea.Right - Toast.FrmToast.Size.Width - Toast.GetHorizontalMargin(),
                                workingArea.Bottom - Toast.FrmToast.Size.Height - Toast.GetVerticalMargin());
                        }
                        else
                        {
                            var collection = ToastCollection.GetBottomRightToasts();
                            var enumerable = collection as List<Toast> ?? collection.ToList();

                            if (enumerable.Count == 0)
                            {
                                Toast.FrmToast.Location = new Point(workingArea.Right - Toast.FrmToast.Size.Width - Toast.GetHorizontalMargin(),
                                    workingArea.Bottom - Toast.FrmToast.Size.Height - Toast.GetVerticalMargin());
                            }
                            else if (enumerable.Count < 3)
                            {
                                Toast.FrmToast.Location = new Point(workingArea.Right - Toast.FrmToast.Size.Width - Toast.GetHorizontalMargin(),
                                    workingArea.Bottom - Toast.FrmToast.Size.Height - Toast.FrmToast.Size.Height * ToastCollection.Count - Toast.GetVerticalMargin() * ToastCollection.Count - Toast.GetVerticalMargin());
                            }
                        }
                    }
                    break;
            }
        }

    }
}
