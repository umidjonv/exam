using System;
using System.Windows.Forms;
using EX.Desktop.Helpers;
using EX.Desktop.Properties;

namespace EX.Desktop.Forms
{
    public partial class ProxyForm : Form
    {
        public ProxyForm()
        {
            InitializeComponent();
        }

        public static bool Detect()
        {
            var form = new ProxyForm();

            return form.ShowDialog() == DialogResult.OK;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Cursor = Cursors.WaitCursor;

            Settings.Default.IsDefault = chbxDefault.Checked;
            Settings.Default.Host = tbxHost.Text;
            Settings.Default.Port = (int) numPort.Value;
            Settings.Default.User = tbxUser.Text;
            Settings.Default.Password = tbxPassword.Text;
            Settings.Default.Save();

            DialogResult = ProxyHelper.Call() ? DialogResult.OK : DialogResult.Cancel;

            Cursor = Cursors.Default;
            Enabled = true;
        }

        private void ProxyForm_Load(object sender, EventArgs e)
        {
            tbxHost.Text = Settings.Default.Host;
            numPort.Value = Settings.Default.Port;
            tbxUser.Text = Settings.Default.User;
            tbxPassword.Text = Settings.Default.Password;
            chbxDefault.Checked = Settings.Default.IsDefault;

            gpxOption.Enabled = !chbxDefault.Checked;
        }

        private void ChbxDefault_CheckedChanged(object sender, EventArgs e)
        {
            gpxOption.Enabled = !chbxDefault.Checked;
        }
    }
}