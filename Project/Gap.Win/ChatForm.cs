using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gap.Win
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
        }

        public string OwnerUsername { get; set; }

        public Form1 MainForm { get; set; }

        public void AddMessage(string username, string message)
        {
            rtMessages.Text += string.Format(
                "{0}<{1}>: {2}{3}",
                username,
                DateTime.Now.ToString("T"),
                message,
                Environment.NewLine);
        }

        private void SendMessage(string message)
        {
            MainForm.SendMessage(this.Text, message);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text.Trim()))
                return;

            SendMessage(txtMessage.Text);
            AddMessage(OwnerUsername, txtMessage.Text);

            txtMessage.Text = string.Empty;
            txtMessage.Focus();
        }
    }
}
