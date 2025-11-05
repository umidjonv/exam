using System;
using System.Windows.Forms;
using EX.Common.Helpers;
using EX.Desktop.Models;
using EX.Desktop.Services;
using Serilog;

namespace EX.Desktop.Forms
{
    public partial class LoginForm : Form
    {

        private readonly MembershipService _membership;

        private readonly SessionService _session;

        private readonly SettingsForm _settings;

        public LoginForm(SettingsForm settings, MembershipService membership, SessionService session)
        {
            _settings = settings;
            _membership = membership;
            _session = session;

            InitializeComponent();

            tbxUsername.GotFocus += TbxUsername_GotFocus;
            tbxUsername.LostFocus += TbxUsername_LostFocus;
            
            tbxPassword.GotFocus += TbxPassword_GotFocus; 
            tbxPassword.LostFocus += TbxPassword_LostFocus;
        }

        private void SetEnableButtons()
        {
            btnLogin.Enabled = !string.IsNullOrWhiteSpace(tbxPassword.Text) &&
                               !string.IsNullOrWhiteSpace(tbxUsername.Text) &&
                               tbxPassword.Text != "Пароль" &&
                               tbxUsername.Text != "Имя пользователя";
        }

        private void TbxPassword_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxPassword.Text))
            {
                tbxPassword.Text = "Пароль";
                tbxPassword.PasswordChar = '\0';
            }
        }

        private void TbxPassword_GotFocus(object sender, EventArgs e)
        {
            if (tbxPassword.Text == "Пароль")
            {
                tbxPassword.Clear();
                tbxPassword.PasswordChar = '*';
            }
        }

        private void TbxUsername_GotFocus(object sender, EventArgs e)
        {
            if (tbxUsername.Text == "Имя пользователя") tbxUsername.Clear();
        }

        private void TbxUsername_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxUsername.Text)) tbxUsername.Text = "Имя пользователя";
        }

        private void LinkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = $"{AppSettings.IdUrl}/Register";

            Log.Information($"Redirect to {link}");

            ProcessHelper.OpenBrowser(link);
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = tbxUsername.Text.Trim();
            var password = tbxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
                throw new Exception("Имя пользователя и пароль не требуется!");

            if (username == "Имя пользователя" ||
                password == "Пароль")
                throw new Exception("Имя пользователя или пароль неверны!");

            try
            {
                var model = new LoginInput
                {
                    UserName = username,
                    Password = password
                };

                UseWaitCursor = true;
                btnLogin.Enabled = false;

                var token = await _membership.GetToken(model.UserName, model.Password);
                var info = await _membership.GetInfo(token.AccessToken);

                await _session.Set(token.AccessToken, token.SessionState);

                var result = new AuthResult
                {
                    Token = token,
                    Info = info
                };
                IdentityContext.CurrentUser = result.Info;
                IdentityContext.UserToken = result.Token;
                 
                Hide();
                _settings.Login = this;
                _settings.ShowDialog();
            }
            catch (Exception exp)
            {
                throw new Exception(exp.InnerException?.Message ?? exp.Message);
            }
            finally
            {

                tbxUsername.Text = "Имя пользователя";
                tbxPassword.Text = "Пароль";
                tbxPassword.PasswordChar = '\0';

                UseWaitCursor = false;
                btnLogin.Enabled = false;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {  
            Log.Information($"Form '{Text}' loaded");
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Information($"Form '{Text}' closed");

            Application.Exit();
        }

        private void TbxPassword_TextChanged(object sender, EventArgs e)
        {
            SetEnableButtons();
        }

        private void TbxUsername_TextChanged(object sender, EventArgs e)
        {
            SetEnableButtons();
        }

        private void TbxUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            var username = tbxUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Имя пользователя обязательно!");

            if (username == "Имя пользователя")
                throw new Exception("Имя пользователя неверное!");

            btnLogin.PerformClick();
        }

        private void TbxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            var password = tbxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Пароль требуется!");

            if (password == "Пароль")
                throw new Exception("Пароль неверный!");

            btnLogin.PerformClick();
        }
    }
}