using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using EX.Desktop.Forms;
using EX.Desktop.Helpers;
using Serilog;
using Win32Pinvoke;

namespace EX.Desktop
{
    public static class Program
    {
        private static readonly Mutex _mutex = new Mutex(true, AppSettings.AppName);

        private static void RunApp()
        {
            CheckUpdater();

            Bootstrapper.ExecuteScope<LoginForm>(form =>
            {
                Application.Run(form);
            });
        }

        private static void CheckUpdater()
        {

            if (Debugger.IsAttached)
                return;

            var screen = Screen.PrimaryScreen.WorkingArea;
            var width = screen.Width * 80 / 100;
            var height = screen.Height * 60 / 100; 

            AutoUpdater.AppTitle = "UzEx: Examination System";
            AutoUpdater.DownloadPath = AppSettings.AppRoot;
            AutoUpdater.HttpUserAgent = AppSettings.AppName;
            AutoUpdater.InstallationPath = AppSettings.AppRoot;
            AutoUpdater.InstalledVersion = new Version(0, 0, 0, 0);
            AutoUpdater.LetUserSelectRemindLater = true;
            AutoUpdater.Proxy = ProxyHelper.DefaultProxy;
            AutoUpdater.ReportErrors = false;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.ShowRemindLaterButton = true;
            AutoUpdater.ShowSkipButton = true;
            AutoUpdater.Synchronous = true;
            AutoUpdater.UpdateFormSize = new Size(width, height);
            AutoUpdater.UpdateMode = Mode.Forced;
            AutoUpdater.OpenDownloadPage = true;
            AutoUpdater.Start($"{AppSettings.ApiUrl}/setup/updater.xml");

            Thread.Sleep(AppSettings.DelayTime);

        }
         
        public static void Main(string[] args)
        {
            // log
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(Environment.CurrentDirectory, "Logs", "app.log"),
                    rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();
            Log.Information("App start");

            // app
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.ApplicationExit += Application_ApplicationExit;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
         
            if (_mutex.WaitOne(TimeSpan.Zero, true))
            {
                
                if (WinInet.InternetIsConnected() && ProxyHelper.Call())
                {
                    Log.Information("Connected to server.");

                    RunApp();
                }
                else
                {
                    if (MessageBox.Show("Не подключен.", "Сервер", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (ProxyForm.Detect())
                        {
                            Log.Information("Connection: OK.");

                            RunApp();
                        }
                        else
                        {
                            MessageBox.Show("Нет соединения!", "Интернет", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }

                _mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Уже запущен!", "Приложение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Log.Warning("App exit");

            Log.CloseAndFlush();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                var exp = e.Exception;

                MessageBox.Show(exp.InnerException?.Message ?? exp.Message, "Ошибка");
            }
            catch (Exception exp)
            {
                Log.Error(exp.ToString());
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exp = (Exception)e.ExceptionObject;

                MessageBox.Show( exp.InnerException?.Message ?? exp.Message, "Ошибка");
            }
            catch (Exception exp)
            {
                Log.Error(exp.ToString());
            }
        }
    }
}