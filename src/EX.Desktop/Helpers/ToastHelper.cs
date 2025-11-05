using EasyToast;
using EX.Desktop.Properties;
using System.Windows.Forms;

namespace EX.Desktop.Helpers
{
    public static class ToastHelper
    {

        public static void Notify(this Form form, string title, string description)
        {
            var toast = ToastBuilder.Create(form);
            toast.SetDescription(description);
            toast.SetThumbnail(Resources.comp);
            toast.SetTitle(title);
            
            toast.Show();
        }

    }
}
