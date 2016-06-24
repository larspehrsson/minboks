using System;
using System.Linq;
using System.Windows.Forms;

namespace MinBoks
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            tbUserId.Text = Properties.Settings.Default["brugernavn"].ToString();
            tbPassword.Text = Properties.Settings.Default["password"].ToString();
            tbActivationCode.Text = Properties.Settings.Default["aktiveringskode"].ToString();
        }

        public void Log(string log)
        {
            logBox.Items.Add(DateTime.Now.ToLongTimeString() + "  " + log);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var deviceid = Properties.Settings.Default["deviceid"].ToString();
            if (string.IsNullOrEmpty(deviceid))
                deviceid = Guid.NewGuid().ToString();

            var response = Properties.Settings.Default["response"].ToString();
            if (string.IsNullOrEmpty(response))
                response = GetRandomHexNumber(64);


            Properties.Settings.Default["deviceid"] = deviceid;
            Properties.Settings.Default["response"] = response;
            Properties.Settings.Default["brugernavn"] = tbUserId.Text;
            Properties.Settings.Default["password"] = tbPassword.Text;
            Properties.Settings.Default["aktiveringskode"] = tbActivationCode.Text;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file

            var account = new Eboks.Account
            {
                UserId = tbUserId.Text,
                Password = tbPassword.Text,
                ActivationCode = tbActivationCode.Text,
                DeviceId = deviceid,
                response = response,
            };

            var api = new Eboks.Api();
            api.setguireference(this);
            api.GetSessionForAccountRest(account);
            api.DownloadAll(account);
        }

        static Random random = new Random();
        public static string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result.ToLower();
            return (result + random.Next(16).ToString("x")).ToLower();
        }

    }
}
