using System;
using System.Drawing;
using System.Windows.Forms;
using Accord.Video.FFMPEG;

namespace EX.VideoMaker
{
    public partial class Form1 : Form
    {
        private readonly Timer _timer;
        private readonly VideoFileWriter _writer;

        public Form1()
        {
            InitializeComponent();

            _timer = new Timer
            {
                Interval = 20
            };
            _timer.Tick += TimerOnTick;
            _writer = new VideoFileWriter();
            _writer.Open("result.mp4", Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, 25, VideoCodec.MPEG4, 1000000);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {  
            var bp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            var gr = Graphics.FromImage(bp);
            gr.CopyFromScreen(0, 0, 0, 0, new Size(bp.Width, bp.Height));
            var old = pictureBox1.Image;
            pictureBox1.Image = bp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            _writer.WriteVideoFrame(bp);
            if (old != null) 
            old.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _timer.Stop();
            _writer.Close();
        }
    }
}