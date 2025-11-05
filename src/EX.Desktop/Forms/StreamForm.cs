using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using EX.Desktop.Helpers;
using EX.Desktop.Properties;
using EX.Desktop.Services;
using Serilog;
using Image = AForge.Imaging.Image;

namespace EX.Desktop.Forms
{
    public partial class StreamForm : Form
    {
        private readonly StreamService _service;

        private readonly FinishForm _finish;

        private VideoCaptureDevice _video;

        private readonly BackgroundWorker _worker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        public StreamForm(StreamService service, FinishForm finish)
        {
            _service = service;
            _finish = finish;

            InitializeComponent();
        }

        public string AudioDevice { get; set; }

        public string VideoDevice { get; set; }

        public DateTime EndTime { get; set; }

        private void StreamForm_Load(object sender, EventArgs e)
        {
            var workingArea = Screen.GetWorkingArea(this);
            var x = workingArea.Right - Width - 10;
            var y = workingArea.Bottom - Height - 20;
            Location = new Point(x, y);

            if (!string.IsNullOrWhiteSpace(VideoDevice))
            {
                _video = new VideoCaptureDevice(VideoDevice);
                _video.NewFrame += VideoCapture_NewFrame;
                _video.Start();
            }

            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _video?.Stop();

            Hide();

            _finish.Show();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var token = IdentityContext.UserToken;
            var user = IdentityContext.CurrentUser;

            _finish.DoAction(async () =>
            {
                await _service.Stop();
            });

            Task.Run(async () =>
            {
                await _service.Listen(user.Id, token.SessionState, AudioDevice, EndTime, () =>
                {
                    _worker.CancelAsync();
                });
            });
            Log.Information("Stream listening");

            Thread.Sleep(AppSettings.DelayTime);

            while (!_worker.CancellationPending)
            {
                Thread.Sleep(AppSettings.DelayTime);
            }

            e.Cancel = true;
        }

        private void VideoCapture_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                var old = pictureBox1.Image;
                var video = Image.Clone(eventArgs.Frame);

                pictureBox1.Image = video;
                old?.Dispose();
            }
            catch
            {
                pictureBox1.Image = Resources.camera;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            RecorderHelper.Killing();

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _worker.CancelAsync();

            Log.Information($"Form '{Text}' closed");

            base.OnClosed(e);
        }
    }
}