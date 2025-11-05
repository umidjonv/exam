using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EX.CameraWatcher
{
    public partial class MainForm : Form
    {
        private readonly FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCaptureDevice videoCapture;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var workingArea = Screen.GetWorkingArea(this);
            var x = workingArea.Right - Width - 20;
            var y = workingArea.Bottom - Height - 10;

            Location = new Point(x, y);

            videoCapture = filterInfoCollection.Count > 0
                ? new VideoCaptureDevice(filterInfoCollection[0].MonikerString)
                : new VideoCaptureDevice();
            videoCapture.NewFrame += VideoCaptureDevice_NewFrame;
            videoCapture.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var old = pictureBox1.Image;
            var video = AForge.Imaging.Image.Clone(eventArgs.Frame);

            pictureBox1.Image = video;
            old?.Dispose();
        }

        protected override void OnClosed(EventArgs e)
        {
            videoCapture.Stop();
        }


    }
}
