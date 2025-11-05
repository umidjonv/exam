using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using EX.Common.Dtos;
using EX.Common.Extensions;
using EX.Common.Helpers;
using EX.Desktop.Helpers;
using EX.Desktop.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace EX.Desktop.Forms
{
    public partial class SettingsForm : Form
    {

        private readonly AudioForm _audio;

        private readonly IList<SelectItemDto> _audioDevices = new List<SelectItemDto>();

        private readonly CameraForm _camera;

        private readonly NetworkForm _network;

        private readonly IList<SelectItemDto> _cameraDevices = new List<SelectItemDto>();

        private readonly ExamService _exam;

        private readonly MembershipService _membership;

        private readonly SessionService _session;

        private readonly StreamForm _stream;

        private bool _connectedCamera;

        private bool _connectedSound;

        private bool _connectedInternet;

        private HubConnection _timerConnection;
        private HubConnection _examConnection;

        private DateTime _endTime;

        private bool _startedExam;

        private DateTime _startTime;

        private bool Started => _connectedCamera && _connectedSound && _startedExam && _connectedInternet;

        public SettingsForm(AudioForm audio, CameraForm camera, MembershipService membership, SessionService session, ExamService exam, StreamForm stream, NetworkForm network)
        {
            _audio = audio;
            _camera = camera;
            _membership = membership;
            _session = session;
            _exam = exam;
            _stream = stream;
            _network = network;

            InitializeComponent();
        }

        public LoginForm Login { get; set; }

        private void LoadAudioDevices()
        {
            var devices = RecorderHelper.GetAudioDevices();

            foreach (var device in devices)
                _audioDevices.Add(new SelectItemDto(device.Key, device.Value));

            Log.Information($"Audio devices: [{string.Join(",", _audioDevices)}]");

            cbxAudioDevices.DataSource = _audioDevices;
            cbxAudioDevices.DisplayMember = "Text";
            cbxAudioDevices.ValueMember = "Value";

            if (_audioDevices.Count > 0)
            {
                cbxAudioDevices.SelectedIndex = 0;
                btnTestSound.Enabled = true;
            }

        }

        private void LoadCameraDevices()
        {
            var devices = RecorderHelper.GetVideoDevices();

            foreach (var device in devices)
                _cameraDevices.Add(new SelectItemDto(device.Key, device.Value));

            Log.Information($"Video devices: [{string.Join(",", _cameraDevices)}]");

            cbxCameraDevices.DataSource = _cameraDevices;
            cbxCameraDevices.DisplayMember = "Text";
            cbxCameraDevices.ValueMember = "Value";

            if (_cameraDevices.Count > 0)
            {
                cbxCameraDevices.SelectedIndex = 0;
                btnTestCamera.Enabled = true;
            }

        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            var token = IdentityContext.UserToken;
            var audio = (SelectItemDto)cbxAudioDevices.SelectedItem;
            var video = (SelectItemDto)cbxCameraDevices.SelectedItem;

            await _session.Active(token.SessionState, audio?.Text, video?.Text);

            Hide();
            _stream.AudioDevice = audio?.Text;
            _stream.VideoDevice = video?.Value;
            _stream.EndTime = _endTime;

            _stream.Show();
        }

        private void LblLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Information("Log out");

            Task.Run(async () =>
            {
                await _membership.LogOff(IdentityContext.UserToken.AccessToken);
            });

            Login.Show();

            Hide();
        }

        private void BtnTestSound_Click(object sender, EventArgs e)
        {
            var deviceItem = (SelectItemDto)cbxAudioDevices.SelectedItem;
            if (deviceItem == null)
                return;

            _audio.DeviceId = cbxAudioDevices.SelectedIndex;
            _audio.DeviceName = deviceItem.Text;

            if (_audio.ShowDialog(this) == DialogResult.OK)
            {
                _connectedSound = true;

                this.Notify("Аудио", "Подключено устройство!");
            }
            else
            {
                _connectedSound = false;

                this.Notify("Аудио", "Не подключенное устройство!");
            }

            btnStart.Enabled = Started;
        }

        private void BtnTestCamera_Click(object sender, EventArgs e)
        {
            var deviceItem = (SelectItemDto)cbxCameraDevices.SelectedItem;
            if (deviceItem == null)
                return;

            _camera.DeviceId = deviceItem.Value;
            _camera.DeviceName = deviceItem.Text;

            if (_camera.ShowDialog(this) == DialogResult.OK)
            {
                _connectedCamera = true;

                this.Notify("Камера", "Подключено устройство!");
            }
            else
            {
                _connectedCamera = false;

                this.Notify("Камера", "Не подключенное устройство!");
            }

            btnStart.Enabled = Started;
        }

        private void LblUserName_Click(object sender, EventArgs e)
        {
            var link = $"{AppSettings.IdUrl}/manage";

            Log.Information($"Redirect to {link}");

            ProcessHelper.OpenBrowser(link);
        }

        private async void SettingsForm_Load(object sender, EventArgs e)
        {
            Log.Information($"Form '{Text}' loaded");

            lblUserName.Text = IdentityContext.CurrentUser.Name;

            LoadAudioDevices();
            LoadCameraDevices();

            await StartTimer();
        }

        private async Task StartTimer()
        {

            var user = IdentityContext.CurrentUser;
            ExamStateDto status = null;

            try
            {
                status = await _exam.CheckStatus(user.Id);

                _startTime = $"{status.StartTime}".ConvertFullDate();
                _endTime = $"{status.EndTime}".ConvertFullDate();
            }
            catch (Exception exp)
            {
                status = null;

                lblClock.Text = exp.InnerException?.Message ?? exp.Message;
            }

            try
            {
                _timerConnection = new HubConnectionBuilder()
                    .WithUrl($"{AppSettings.ApiUrl}/hub/timer", options =>
                    {
                        options.Proxy = ProxyHelper.DefaultProxy;
                    })
                    .Build();

                _timerConnection.On<DateTime>("time-now", async time =>
                {
                    if (status == null)
                        return;

                    var elapsed = time.ToUniversalTime().Subtract(_startTime);

                    if (elapsed.TotalSeconds >= 0)
                    {
                        status = null;

                        _startedExam = true;
                        btnStart.Enabled = Started;

                        lblClock.Text = @"Экзамен начался";

                        this.Notify("Экзамен", "Начался!");

                        await _timerConnection.StopAsync();
                    }
                    else
                    {
                        lblClock.Text = $@"{elapsed:hh\:mm\:ss}";
                    }
                });

                await _timerConnection.StartAsync();
            }
            catch (Exception exp)
            {
                this.Notify("Сервер", exp.InnerException?.Message ?? exp.Message);
            }

            try
            {
                _examConnection = new HubConnectionBuilder()
                    .WithUrl($"{AppSettings.ApiUrl}/hub/exam", options =>
                    {
                        options.Proxy = ProxyHelper.DefaultProxy;
                    })
                    .Build();

                _examConnection.On<ExamStateDto>($"Assign-{user.Id}", async model =>
                {
                    status = model;

                    if (status == null)
                    {
                        await _examConnection.StopAsync();
                        Close();
                    }
                    else
                    {
                        var startTime = $"{status.StartTime}".ConvertFullDate();
                        var endTime = $"{status.EndTime}".ConvertFullDate();

                        if (startTime != _startTime)
                        {
                            this.ThreadInvoke(() =>
                            {
                                this.Notify("Экзамен", "Время изменилось");
                            });
                        }

                        _startTime = startTime;
                        _endTime = endTime;

                    }
                });

                await _examConnection.StartAsync();
            }
            catch (Exception exp)
            {
                this.Notify("Сервер", exp.InnerException?.Message ?? exp.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Log.Information($"Form '{Text}' closed");

            Hide();
            Login.Show();

            base.OnClosed(e);
        }

        private void BtnInternet_Click(object sender, EventArgs e)
        {
            if (_network.ShowDialog(this) == DialogResult.OK)
            {
                _connectedInternet = true;

                lblSpeedLan.ForeColor = Color.Green;
                lblSpeedLan.Text = "Скорость хорошая.";

                this.Notify("Интернет", "Скорость хорошая.");
            }
            else
            {
                _connectedInternet = false;

                lblSpeedLan.ForeColor = Color.Red;
                lblSpeedLan.Text = "Скорость медленная!";

                this.Notify("Интернет", "Скорость медленная!");
            }

            btnStart.Enabled = Started;
        }
    }
}