using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using Serilog;
using WaveFormRendererLib;
using WaveFormRendererLib.Providers;

namespace EX.Desktop.Forms
{
    public partial class AudioForm : Form
    {
        private const int Waiter = 10;

        private readonly WaveOut _player = new WaveOut();

        private readonly Timer _timer = new Timer
        {
            Interval = 1000,
            Enabled = false
        };

        private BufferedWaveProvider _bufferedWaveProvider;

        private bool _connected;

        private int _counter;

        private string _file;

        private WaveIn _recorder;

        private SavingWaveProvider _savingWaveProvider;

        public AudioForm()
        {
            InitializeComponent();
        }

        public int DeviceId { get; set; }

        public string DeviceName { get; set; }

        private void Start()
        {
            try
            {
                _file = Path.Combine(AppSettings.AppRoot, "Tmp", "audio", Path.ChangeExtension(Path.GetRandomFileName(), ".wav"));
                _recorder = new WaveIn
                {
                    DeviceNumber = DeviceId,
                    BufferMilliseconds = AppSettings.DelayTime,
                    WaveFormat = new WaveFormat(44100, 2)
                };
                _recorder.DataAvailable += RecorderOnDataAvailable;
                _bufferedWaveProvider = new BufferedWaveProvider(_recorder.WaveFormat);
                _savingWaveProvider = new SavingWaveProvider(_bufferedWaveProvider, _file);
                _player.Init(_savingWaveProvider);
                _recorder.StartRecording();
                _player.Play();

                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            catch (Exception exp)
            {
                Stop();

                Log.Error(exp.InnerException?.Message ?? exp.Message);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_counter == Waiter)
            {
                Stop();
            }
            else
            {
                _counter++;

                var image = DrawImage();
                var old = pictureBox1.Image;
                pictureBox1.Image = image;
                old?.Dispose();

                var percent = _counter * 100 / Waiter;
                progressBar1.Value = percent;
            }
        }

        private void Stop()
        {
            _recorder.StopRecording();
            _player.Stop();
            _savingWaveProvider.Dispose();

            _timer.Stop();

            if (_connected && _counter == Waiter)
            {
                DialogResult = DialogResult.OK;

                Log.Information($"Device '{Text}' is connected");
            }
            else
            {
                DialogResult = DialogResult.Cancel;

                Log.Warning($"Device '{Text}' not connected");
            }

            try
            {
                File.Delete(_file);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
            }
        }

        private Image DrawImage()
        {
            var provider = _bufferedWaveProvider.ToSampleProvider();
            var maxPeakProvider = new MaxPeakProvider();
            var rendererSettings = new StandardWaveFormRendererSettings
            {
                Width = Width,
                TopHeight = Height / 2,
                BottomHeight = Height / 2,
                BackgroundColor = Color.Transparent,
                TopPeakPen = new Pen(Color.DarkGreen),
                BottomPeakPen = new Pen(Color.Green)
            };
            var renderer = new WaveFormRenderer();

            maxPeakProvider.Init(provider, 1);

            return renderer.Render(maxPeakProvider, rendererSettings);
        }

        private void RecorderOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            try
            {
                _connected = true;

                _bufferedWaveProvider.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
            }
            catch (Exception exception)
            {
                _connected = false;

                Stop();

                Log.Error(exception.Message);
            }
        }

        private void AudioForm_Load(object sender, EventArgs e)
        {
            Text = DeviceName;

            Log.Information($"Form '{Text}' loaded");

            Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            _counter = 0;
            _connected = false;

            Log.Information($"Form '{Text}' closed");

            base.OnClosed(e);
        }
    }
}