using System;
using System.Windows.Forms;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using EX.Desktop.Properties;
using Serilog;

namespace EX.Desktop.Forms
{
    public partial class CameraForm : Form
    {
        private const int Waiter = 10;

        private readonly Timer _timer = new Timer
        {
            Interval = 1000,
            Enabled = false
        };

        private bool _connected;

        private int _counter = Waiter;

        private VideoCaptureDevice _video;

        public CameraForm()
        {
            InitializeComponent();
        }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        private void Start()
        {
            try
            {
                _video = new VideoCaptureDevice(DeviceId);
                _video.NewFrame += VideoCapture_NewFrame;
                _video.Start();

                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            catch (Exception exp)
            {
                Stop();

                Log.Error(exp.InnerException?.Message ?? exp.Message);
            }
        }

        private void Stop()
        {
            _video.Stop();
            _timer.Stop();

            if (_connected && _counter == 0)
            {
                DialogResult = DialogResult.OK;

                Log.Information($"Device '{Text}' connected");
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                
                Log.Information($"Device '{Text}' not connected");
            }
        }

        private void VideoCapture_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                _connected = true;

                var old = pictureBox1.Image;
                var video = Image.Clone(eventArgs.Frame);
                pictureBox1.Image = video;
                old?.Dispose();
            }
            catch
            {
                _connected = false;

                Stop();

                pictureBox1.Image = Resources.camera;
            }
        }

        private void CameraForm_Load(object sender, EventArgs e)
        {
            Text = DeviceName;
            
            Log.Information($"Form '{Text}' loaded");

            Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_counter == 0)
            {
                Stop();
            }
            else
            {
                --_counter;

                label1.Text = $"{_counter} sec";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _counter = Waiter;
            _connected = false;

            Log.Information($"Form '{Text}' closed");

            base.OnClosed(e);
        }
    }
}