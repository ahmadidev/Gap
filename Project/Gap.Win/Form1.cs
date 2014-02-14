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
    using System.Net;
    using System.Net.Sockets;
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

            Login(name, serverSocket);

            var onlineUsers = GetOnlineUsers(serverSocket);

            this.DisplayOnlineUsers(onlineUsers);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        private void DisplayOnlineUsers(string[] onlineUsers)
        {
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

        private static string[] GetOnlineUsers(Socket serverSocket)
        {
            byte[] buffer;
            buffer = Encoding.ASCII.GetBytes(MakeCommand("getOnlineUsers", string.Empty));
            serverSocket.Send(buffer);
            serverSocket.Receive(buffer);

            string[] onlineUsers = Encoding.ASCII.GetString(buffer).Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return onlineUsers;
        }

        private static void Login(string name, Socket serverSocket)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(MakeCommand("login", name));
            serverSocket.Send(buffer);
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
    }
}
