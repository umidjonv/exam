using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EX.WebCamera
{
    public partial class MainForm : Form
    {
        private readonly VideoCapture capture;
        private Bitmap image;
        private readonly BackgroundWorker worker;

        public MainForm()
        {
            InitializeComponent();

            capture = new VideoCapture();
            capture.Open(0);

            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var old = pictureBox1.Image;
            var video = (Image)image.Clone();
            pictureBox1.Image = video;
            old?.Dispose();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (capture.IsOpened())
            {
                var frame = new Mat();
                capture.Read(frame);

                image = frame.ToBitmap();

                worker.ReportProgress(0);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            worker.CancelAsync();
            capture.Release();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
