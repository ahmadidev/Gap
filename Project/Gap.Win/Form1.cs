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
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows.Forms.VisualStyles;

    public partial class Form1 : Form
    {
        private string clientName;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private Socket serverSocket;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var name = txtUsername.Text;

            serverSocket = Connect();

            Login(name);

            new Thread(this.UpdateOnlineUsersThreadMethod).Start();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        private void UpdateOnlineUsersThreadMethod()
        {
            while (true)
            {
                this.UpdateOnlineUsers();

                Thread.Sleep(500);
            }
        }

        private void UpdateOnlineUsers()
        {
            var onlineUsers = GetOnlineUsers();

            this.DisplayOnlineUsers(onlineUsers);
        }

        private void DisplayOnlineUsers(string[] onlineUsers)
        {
            if (lbOnlineUsers.InvokeRequired)
            {
                lbOnlineUsers.Invoke(new MethodInvoker(() => this.DisplayOnlineUsers(onlineUsers)));
                return;
            }

            lbOnlineUsers.Items.Clear();
            foreach (var onlineUserName in onlineUsers)
            {
                this.lbOnlineUsers.Items.Add(onlineUserName);
            }
        }

        private static Socket Connect()
        {
            IPAddress serverIpAddress = IPAddress.Loopback;
            const int ServerPort = 23605;
            IPEndPoint serverIpEndPoint = new IPEndPoint(serverIpAddress, ServerPort);
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Connect(serverIpEndPoint);
            return serverSocket;
        }

        private string[] GetOnlineUsers()
        {
            string response = this.SendRequest(MakeCommand("getOnlineUsers", string.Empty));

            if (response == "empty")
                return new string[0];

            string[] onlineUsers = response.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            return onlineUsers;
        }

        private void Login(string name)
        {
            this.SendRequest(MakeCommand("login", name));
        }

        private void Logout()
        {
            byte[] buffer = Encoding.ASCII.GetBytes(MakeCommand("logout", string.Empty));
            this.serverSocket.Send(buffer);
        }

        private static string MakeCommand(string name, string param)
        {
            return string.Format("{0};{1}", name, param);
        }

        private string SendRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);

            serverSocket.Send(buffer);

            buffer = new byte[1024];

            int receivedLength = serverSocket.Receive(buffer);

            string response = Encoding.ASCII.GetString(buffer, 0, receivedLength);

            return response;
        }
    }
}
