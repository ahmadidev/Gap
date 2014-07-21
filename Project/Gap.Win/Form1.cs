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
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms.VisualStyles;

    using Gap.Network;

    public partial class Form1 : Form
    {
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

        private Socket _transmitterSocket;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //var clientThread = new Thread(StartClient);
            //clientThread.Name = "Client_clientThread";
            //clientThread.IsBackground = true;
            //clientThread.Start();

            Task.Run(() => StartClient());
        }

        private void StartClient()
        {
            var name = txtUsername.Text;

            this._transmitterSocket = Connect();

            this.receiverSocket = InitialReceiverSocket();
            int receiverPort = ((IPEndPoint)receiverSocket.LocalEndPoint).Port;


            //var notifyThread = new Thread(this.HandleServerNotifies);
            //notifyThread.Name = "Client_notifyThread";
            //notifyThread.IsBackground = true;
            //notifyThread.Start();

            Task.Run(() => HandleServerNotifies());

            IntroReceiverPort(receiverPort);

            Login(name);
            UpdateOnlineUsers();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.Logout();

            lbLogs.Items.Clear();
            lbOnlineUsers.Items.Clear();
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


            //this.AddLog("New user loggedin:");

            //foreach (var onlineUserName in onlineUsers)
            //{
            //    this.AddLog(onlineUserName);
            //}
        }

        private static Socket Connect()
        {
            TcpClient tcpClient = new TcpClient();

            Configuration configuration = Configuration.Load();

            tcpClient.Connect(configuration.ServerAddress, configuration.ServerPort);

            return tcpClient.Client;
        }

        private string[] GetOnlineUsers()
        {
            return this.SendRequest("getOnlineUsers").Parameters;
        }

        private void Login(string name)
        {
            this.SendRequest("login", name);
        }

        private void Logout()
        {
            if (this._transmitterSocket != null && this._transmitterSocket.Connected)
            {
                Task.Run(() =>
                {
                    this.SendRequest("logout");
                    this.CloseSockets();
                });
            }
        }

        private void CloseSockets()
        {
            //this.transmitterSocket.Shutdown(SocketShutdown.Both);
            //this.transmitterSocket.Disconnect(false);
            //this._transmitterSocket.Dispose();
        }

        private Gap.Network.Message SendRequest(string requestName, string parameter)
        {
            var requestMessage = new Gap.Network.Message { Name = requestName, Parameters = new string[] { parameter } };

            return SendRequest(requestMessage);
        }

        private Gap.Network.Message SendRequest(string requestName)
        {
            var requestMessage = new Gap.Network.Message { Name = requestName };

            return SendRequest(requestMessage);
        }

        private Gap.Network.Message SendRequest(Gap.Network.Message requestMessage)
        {
            Gap.Network.Message.Send(_transmitterSocket, requestMessage);

            return Gap.Network.Message.Receive(_transmitterSocket);
        }

        //Notify

        private Socket receiverSocket;

        private Socket serverTransmitterSocket;

        private static Socket InitialReceiverSocket()
        {
            TcpListener clienTcpListener = new TcpListener(IPAddress.Any, 0);
            clienTcpListener.Start(1);

            return clienTcpListener.Server;
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

            //this.serverTransmitterSocket.Shutdown(SocketShutdown.Both);
            this.serverTransmitterSocket.Close();

            //this.receiverSocket.Shutdown(SocketShutdown.Both);
            this.receiverSocket.Close();
        }

        public string ProcessNotify()
        {
            var responseMessage = Gap.Network.Message.Receive(serverTransmitterSocket);

            switch (responseMessage.Name)
            {
                case "debug":
                    AddLog(string.Format("Server has sent a debug message: {0}", responseMessage.Parameters[0]));
                    break;
                case "bye":
                    AddLog("Server wants to finish this session");
                    break;
                case "getOnlineUsers":
                    this.DisplayOnlineUsers(responseMessage.Parameters);
                    break;
                //case "logout":
                //    this.Logout();
                //    break;
                //case "IntroReceiverPort":
                //    int clientReceiverPort = int.Parse(notifyParameter);
                //    ConnectToClient(clientReceiverPort);
                //    break;
            }

            Gap.Network.Message.Send(serverTransmitterSocket, "OK");

            return responseMessage.Name;
        }

        private void IntroReceiverPort(int receiverPort)
        {
            this.SendRequest("IntroReceiverPort", receiverPort.ToString());
        }

        private void AddLog(string log)
        {
            //Console.WriteLine(log);
            if (lbLogs.InvokeRequired)
            {
                lbLogs.Invoke(new MethodInvoker(() => this.AddLog(log)));
            }
            else
            {
                lbLogs.Items.Add(log);
            }
        }
    }
}
