using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EX.Desktop.Services;
using Serilog;

namespace EX.Desktop.Forms
{
    public partial class NetworkForm : Form
    {

        private readonly SpeedService _service;

        private void ReportProgress(string status)
        {
            statusLabel.Invoke(new Action(() =>
            {
                statusLabel.Text = status;
            }));
        }

        private async Task DownloadSpeed()
        {
            ReportProgress("Скачивание ...");
             
            for (var i = 0; i < AppSettings.SpeedTime; i++)
            {
                try
                {
                    var beginTime = DateTime.Now;
                    var quota = await _service.Download();
                    var endTime = DateTime.Now;
                    var speed = (double)quota / 1024 / 1024 / (endTime - beginTime).TotalSeconds;
                     
                    if (speed > AppSettings.LanLimit)
                    {
                        ReportProgress($"Попытка  {i + 1} раз: {speed:F} MB/sec");
                    }
                    else
                    {
                        //throw new Exception($"Попытка {i + 1} раз: TIMEOUT!"); 
                    }
                }
                catch (Exception e)
                {
                    ReportProgress(e.Message);
                }

                Application.DoEvents();
            }

        }

        private async Task UploadSpeed()
        {

            ReportProgress("Загрузка ...");

            var path = Path.Combine(AppSettings.AppRoot, "tmp", "uzex");
            var data = File.ReadAllBytes(path);

            for (var i = 0; i < AppSettings.SpeedTime; i++)
            {
                try
                {
                    var beginTime = DateTime.Now;

                    await _service.Upload(data);

                    var endTime = DateTime.Now;
                    var speed = (double)data.LongLength / 1024 / 1024 / (endTime - beginTime).TotalSeconds;
                       
                    if (speed > AppSettings.LanLimit)
                    {
                        ReportProgress($"Попытка  {i + 1} раз: {speed:F} MB/sec");
                    }
                    else
                    {
                        //throw new Exception($"Попытка {i + 1} раз: TIMEOUT!"); 
                    }
                }
                catch (Exception e)
                {
                    ReportProgress(e.Message);
                }

                Application.DoEvents();
            }
             
        }

        private void ReportLan()
        {
            ReportProgress("Сеть ...");

            var adapters = NetworkInterface.GetAllNetworkInterfaces()
                .Where(a => a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
            var speed = adapters.Average(a => a.Speed) / 1024 / 1024 / 1024;

            ReportProgress($"Скорость : {speed:F} MB/sec");
        }

        private void ToPing()
        {
            ReportProgress("Сервер ...");

            for (var i = 0; i < AppSettings.SpeedTime; i++)
            {

                var ping = new Ping();
                var beginTime = DateTime.Now;
                var hostAddress = AppSettings.ApiUrl.Replace("http://", "");
                var portIndex = hostAddress.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                var ip = portIndex >= 0 ? hostAddress.Remove(portIndex, hostAddress.Length - portIndex) : hostAddress;
                var reply = ping.Send(ip, AppSettings.DelayTime);
                var endTime = DateTime.Now;

                if (reply != null &&
                    reply.Status == IPStatus.Success)
                {
                    var speed = (endTime - beginTime).TotalSeconds;

                    if (speed > 0)
                    {
                        ReportProgress($"Попытка {i + 1} раз: SUCCESS.");
                    }
                    else
                    {
                        //throw new Exception($"Попытка {i + 1} раз: TIMEOUT!"); 
                    }
                }

            } 

        }

        public NetworkForm(SpeedService service)
        {
            InitializeComponent();

            _service = service;
        }

        protected override void OnClosed(EventArgs e)
        {
            Log.Information($"Form '{Text}' loaded");

            base.OnClosed(e);
        }

        private async void NetworkForm_Load(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;

                ToPing();
                ReportLan();
                await DownloadSpeed();
                await UploadSpeed();

                DialogResult = DialogResult.OK;
            }
            catch (Exception exp)
            {
                ReportProgress(exp.Message);

                Thread.Sleep(AppSettings.DelayTime);

                DialogResult = DialogResult.Cancel;
            }
            finally
            {
                UseWaitCursor = false;
            }

        }

    }
}