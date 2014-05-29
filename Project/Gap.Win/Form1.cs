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

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Logout();
        }

        private Socket transmitterSocket;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var name = txtUsername.Text;

            this.transmitterSocket = Connect();

            this.receiverSocket = InitialReceiverSocket();
            int receiverPort = ((IPEndPoint)receiverSocket.LocalEndPoint).Port;
            new Thread(this.HandleServerNotifies).Start();
            IntroReceiverPort(receiverPort);

            Login(name);
            UpdateOnlineUsers();

            //new Thread(this.UpdateOnlineUsersThreadMethod).Start();
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
            this.SendRequest(MakeCommand("logout", string.Empty));
        }

        private static string MakeCommand(string name, string param)
        {
            return string.Format("{0};{1}", name, param);
        }

        private string SendRequest(string request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(request);

            this.transmitterSocket.Send(buffer);

            buffer = new byte[1024];

            int receivedLength = this.transmitterSocket.Receive(buffer);

            string response = Encoding.ASCII.GetString(buffer, 0, receivedLength);

            return response;
        }

        //Notify

        private Socket receiverSocket;

        private Socket serverTransmitterSocket;

        private static Socket InitialReceiverSocket()
        {
            IPAddress clientIpAddress = IPAddress.Any;
            IPEndPoint clientIpEndPoint = new IPEndPoint(clientIpAddress, 0);
            Socket clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Bind(clientIpEndPoint);
            clientSocket.Listen(1);

            return clientSocket;
        }

        private void HandleServerNotifies()
        {
            this.serverTransmitterSocket = this.receiverSocket.Accept();

            string notifyName;

            do
            {
                notifyName = this.ProcessNotify();
            }
            while (notifyName != "bye");

            this.serverTransmitterSocket.Shutdown(SocketShutdown.Both);
            this.serverTransmitterSocket.Close();

            this.receiverSocket.Shutdown(SocketShutdown.Both);
            this.receiverSocket.Close();
        }

        public string ProcessNotify()
        {
            byte[] buffer = new byte[1024];

            int messageLength = this.serverTransmitterSocket.Receive(buffer, SocketFlags.None);

            string[] notifyParts = Encoding.ASCII.GetString(buffer, 0, messageLength).Split(';');

            string
                notifyName = notifyParts[0],
                notifyParameter = notifyParts[1];

            string response = "OK";

            switch (notifyName)
            {
                case "debug":
                    Console.WriteLine("Server has sent a debug message: {0}", notifyParameter);
                    break;
                case "bye":
                    Console.WriteLine("Server wants to finish this session.");
                    break;
                //case "getOnlineUsers":
                //    response = this.GetOnlineUsers();
                //    break;
                //case "logout":
                //    this.Logout();
                //    break;
                //case "IntroReceiverPort":
                //    int clientReceiverPort = int.Parse(notifyParameter);
                //    ConnectToClient(clientReceiverPort);
                //    break;
            }


            buffer = Encoding.ASCII.GetBytes(response);

            this.serverTransmitterSocket.Send(buffer);

            return notifyName;
        }

        private void IntroReceiverPort(int receiverPort)
        {
            this.SendRequest(MakeCommand("IntroReceiverPort", receiverPort.ToString()));
        }

    }
}
