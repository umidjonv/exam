using System;

namespace EasyToast
{
    public class ToastChangedEventArgs : EventArgs
    {
        private readonly Toast _toast;

        public Toast Toast => _toast;

        public ToastChangedEventArgs(Toast toast)
        {
            _toast = toast;
        }
    }
}
