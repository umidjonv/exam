using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyToast
{
    internal partial class ToastForm : Form
    {
        private bool _hasImageSet;

        private const int AW_SLIDE = 0X40000;

        private const int AW_HOR_POSITIVE = 0X1;

        private const int AW_HOR_NEGATIVE = 0X2;

        private const int AW_HIDE = 0x00010000;

        private const int AW_ACTIVATE = 0x00020000;

        private const int AW_BLEND = 0X80000;

        private const int AW_CENTER = 0x00000010;

        private byte _counter = 2;

        public bool IsAsync = false;

        public Theme Theme;

        public CloseStye CloseStyle;

        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        internal ToastForm()
        {
            InitializeComponent();
            HorizontalMargin = 10;
            VerticalMargin = 10;

            var workingArea = Screen.GetWorkingArea(this);

            Location = new Point(workingArea.Right - Size.Width - HorizontalMargin,
                workingArea.Bottom - Size.Height - VerticalMargin);
        }

        internal Toast Toast { get; set; }

        internal bool IsShown { get; private set; }

        [DefaultValue(Duration.LENGTH_SHORT)]
        internal Duration Duration { get; set; }

        [DefaultValue(Animation.FADE)]
        internal Animation Animation { get; set; }

        [DefaultValue("")]
        internal string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        [DefaultValue("")]
        internal string Description
        {
            get => lblDescription.Text;
            set => lblDescription.Text = value;
        }

        internal Image Thumbnails
        {
            get => picImage.Image;
            set
            {
                picImage.Image = value;
                Invalidate();
                if (value != null)
                {
                    _hasImageSet = true;
                }

            }
        }

        [DefaultValue(false)]
        internal bool IsMuted { get; set; } = false;

        internal int HorizontalMargin { get; }

        internal int VerticalMargin { get; }

        private async void FrmToast_Load(object sender, EventArgs e)
        {
            if (IsAsync)
            {
                await Task.Yield();
            }

            switch (CloseStyle)
            {
                case CloseStye.ClickEntire:
                    textContainer.Panel1Collapsed = true;
                    break;
                case CloseStye.Button:
                    textContainer.Panel1Collapsed = false;
                    break;
                case CloseStye.ButtonAndClickEntire:
                    textContainer.Panel1Collapsed = false;
                    break;
            }

            switch (Duration)
            {
                case Duration.LENGTH_SHORT:
                    _counter = 2;
                    break;
                case Duration.LENGTH_LONG:
                    _counter = 3;
                    break;
            }

            if (!IsMuted)
            {
                //PlaySound();
            }
            SetTheme();
            switch (Animation)
            {
                case Animation.FADE:
                    FadeIn();
                    break;
                case Animation.SLIDE:
                    AnimateWindow(Handle, 250, AW_SLIDE | AW_HOR_NEGATIVE | AW_ACTIVATE);
                    break;
            }

            PlaySound();
        }

        private async void PlaySound()
        {
            await Task.Factory.StartNew(() =>
            {

                var sound = EasyToast.Properties.Resources.notify;
                var player = new SoundPlayer(sound);
                player.Play();
            });
        }

        private void SetTheme()
        {
            switch (Theme)
            {
                case Theme.Dark:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.DarkScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.DarkScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.DarkScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.DarkScheme.GetBackgroundColor();
                    break;
                case Theme.Light:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.LightScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.LightScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.LightScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.LightScheme.GetBackgroundColor();
                    break;
                case Theme.PrimaryLight:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.PrimaryLightScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.PrimaryLightScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.PrimaryLightScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.PrimaryLightScheme.GetBackgroundColor();
                    break;
                case Theme.SuccessLight:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.SuccessLightScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.SuccessLightScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.SuccessLightScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.SuccessLightScheme.GetBackgroundColor();
                    break;
                case Theme.WarningLight:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.WarningLightScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.WarningLightScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.WarningLightScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.WarningLightScheme.GetBackgroundColor();
                    break;
                case Theme.ErrorLight:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.ErrorLightScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.ErrorLightScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.ErrorLightScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.ErrorLightScheme.GetBackgroundColor();
                    break;
                case Theme.PrimaryDark:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.PrimaryDarkScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.PrimaryDarkScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.PrimaryDarkScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.PrimaryDarkScheme.GetBackgroundColor();
                    break;
                case Theme.SuccessDark:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.SuccessDarkScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.SuccessDarkScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.SuccessDarkScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.SuccessDarkScheme.GetBackgroundColor();
                    break;
                case Theme.WarningDark:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.WarningDarkScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.WarningDarkScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.WarningDarkScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.WarningDarkScheme.GetBackgroundColor();
                    break;
                case Theme.ErrorDark:
                    lblTitle.ForeColor = ThemeBuilder.BuiltinScheme.ErrorDarkScheme.GetForegroundColor();
                    btnClose.ForeColor = ThemeBuilder.BuiltinScheme.ErrorDarkScheme.GetForegroundColor();
                    BackColor = ThemeBuilder.BuiltinScheme.ErrorDarkScheme.GetBackgroundColor();
                    btnClose.FlatAppearance.BorderColor = ThemeBuilder.BuiltinScheme.ErrorDarkScheme.GetBackgroundColor();
                    break;
                case Theme.Custom:
                    if (ThemeBuilder.CustomScheme == null)
                    {
                        throw new NullReferenceException($"You must create your scheme before set custom theme. Use ${nameof(ThemeBuilder.CreateCustomScheme)}() to create a custom scheme");
                    }
                    else
                    {
                        lblTitle.ForeColor = ThemeBuilder.CustomScheme.GetForegroundColor();
                        btnClose.ForeColor = ThemeBuilder.CustomScheme.GetForegroundColor();
                        BackColor = ThemeBuilder.CustomScheme.GetBackgroundColor();
                        btnClose.FlatAppearance.BorderColor = ThemeBuilder.CustomScheme.GetBackgroundColor();
                    }
                    break;
            }
        }

        private void FrmToast_Shown(object sender, EventArgs e)
        {

            IsShown = true;
            tmrClose.Start();
        }

        private void FrmToast_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (Animation)
            {
                case Animation.FADE:
                    AnimateWindow(Handle, 250, AW_BLEND | AW_HIDE);
                    break;
                case Animation.SLIDE:
                    AnimateWindow(Handle, 250, AW_SLIDE | AW_HOR_POSITIVE | AW_HIDE);
                    break;
            }
        }

        private void FrmToast_Click(object sender, EventArgs e)
        {
            switch (CloseStyle)
            {
                case CloseStye.ClickEntire:

                case CloseStye.ButtonAndClickEntire:
                    Close();
                    break;
                case CloseStye.Button:
                    return;
            }

        }

        private void LblCaption_Click(object sender, EventArgs e)
        {
            switch (CloseStyle)
            {
                case CloseStye.ClickEntire:

                case CloseStye.ButtonAndClickEntire:
                    Close();
                    break;
                case CloseStye.Button:
                    return;
            }
        }

        private void MainContainer_Panel2_Click(object sender, EventArgs e)
        {
            switch (CloseStyle)
            {
                case CloseStye.ClickEntire:

                case CloseStye.ButtonAndClickEntire:
                    Close();
                    break;
                case CloseStye.Button:
                    return;
            }
        }

        private void MainContainer_Panel1_Click(object sender, EventArgs e)
        {
            switch (CloseStyle)
            {
                case CloseStye.ClickEntire:

                case CloseStye.ButtonAndClickEntire:
                    Close();
                    break;
                case CloseStye.Button:
                    return;
            }
        }

        private void PicImage_Click(object sender, EventArgs e)
        {
            switch (CloseStyle)
            {
                case CloseStye.ClickEntire:

                case CloseStye.ButtonAndClickEntire:
                    Close();
                    break;
                case CloseStye.Button:
                    return;
            }
        }

        private void TmrClose_Tick(object sender, EventArgs e)
        {
            _counter--;
            if (_counter != 0) return;
            tmrClose.Stop();
            Close();
        }

        private async void FadeIn()
        {
            Opacity = 0;
            while (Opacity < 1.0)
            {
                await Task.Delay(3);
                Opacity += 0.05;
            }
            Opacity = 1;
        }

        private SizeF CalculateString()
        {
            if (string.IsNullOrEmpty(lblTitle.Text)) return SizeF.Empty;
            using (var g = CreateGraphics())
            {
                var size = g.MeasureString(lblTitle.Text, lblTitle.Font);
                return size;
            }
        }

        private void FrmToast_FormClosed(object sender, FormClosedEventArgs e)
        {
            ToastManager.ToastCollection.Remove(Toast);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
