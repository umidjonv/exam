using Serilog;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using EX.Desktop.Helpers;

namespace EX.Desktop.Forms
{
    public partial class FinishForm : Form
    {

        private Action _action;

        private readonly BackgroundWorker _worker = new BackgroundWorker
        {
            WorkerReportsProgress = false,
            WorkerSupportsCancellation = true
        };

        public FinishForm()
        {
            InitializeComponent();
        }

        public void DoAction(Action action)
        {
            _action = action;
        }

        protected override void OnClosed(EventArgs e)
        {
            Log.Information($"Form '{Text}' closed");

            base.OnClosed(e);
        }

        private void FinishForm_Load(object sender, EventArgs e)
        {
            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ThreadInvoke(() =>
            {
                this.Notify("Результат", "Все результаты тестов отправлены на сервер.");
            });

            Thread.Sleep(AppSettings.DelayTime);

            Environment.Exit(Environment.ExitCode);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _action();

            Thread.Sleep(AppSettings.DelayTime);
        }

    }
}
