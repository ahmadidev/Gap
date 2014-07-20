using System;
using System.Collections.Generic;
using System.Linq;

namespace Gap.Server
{
    using System.Net;
    using System.Net.Sockets;

    using Gap.Network;

    class User : IDisposable
    {
        public enum UserStatus
        {
            Connected,
            Loggedin,
            Loggedout,
            //Disconnected            
        }

        private static readonly List<User> OnlineUsers = new List<User>();

        public string Name { get; set; }

        public Socket ClientTransmitterSocket { get; set; }

        public Socket ClientReceiverSocket { get; set; }

        public UserStatus Status { get; set; }

        public User(Socket socket)
        {
            this.Status = UserStatus.Connected;
            this.ClientTransmitterSocket = socket;

            OnlineUsers.Add(this);
        }

        public void Login(string name)
        {
            this.Status = UserStatus.Loggedin;
            this.Name = name;

            var user = OnlineUsers.Single(x => Equals(x.ClientTransmitterSocket, this.ClientTransmitterSocket));
            user.Name = name;

            Console.WriteLine("{0} is loggedin.", user.Name);

            //Test notify!
            this.SendNotifyToClient(ClientReceiverSocket, "debug", "You are now Online!\nWelcome my good client... :)");

            BroadCastUserChange();
        }

        public void Logout()
        {
            var user = OnlineUsers.Single(x => x.Name == Name);
            OnlineUsers.Remove(user);

            this.Status = UserStatus.Loggedout;


            //Send bye notify!
            this.SendNotifyToClient(ClientReceiverSocket, "bye");

            BroadCastUserChange();
        }

        public void ProcessRequest()
        {
            var requestMessage = Message.Receive(this.ClientTransmitterSocket);

            var responseMessage = new Message();

            switch (requestMessage.Name)
            {
                case "login":
                    this.Login(requestMessage.Parameters[0]);
                    break;
                case "getOnlineUsers":
                    responseMessage.Parameters = OnlineUsersButMe().Select(x => x.Name).ToArray();
                    break;
                case "logout":
                    this.Logout();
                    break;
                case "IntroReceiverPort":
                    int clientReceiverPort = int.Parse(requestMessage.Parameters[0]);
                    ConnectToClient(clientReceiverPort);
                    break;
            }

            Message.Send(ClientTransmitterSocket, responseMessage);
        }

        //Notify

        private void ConnectToClient(int clientReceiverPort)
        {
            IPAddress clientReceiverSocketIp = ((IPEndPoint)this.ClientTransmitterSocket.LocalEndPoint).Address;

            TcpClient clientTcpClient = new TcpClient();
            clientTcpClient.Connect(clientReceiverSocketIp, clientReceiverPort);

            ClientReceiverSocket = clientTcpClient.Client;
        }

        private void SendNotifyToClient(Socket clientSocket, string name, params string[] parameters)
        {
            Message.Send(clientSocket, name, parameters);

            var responseMessage = Message.Receive(clientSocket);
        }

        private void BroadCastUserChange()
        {
            var onlineUsersButMe = OnlineUsersButMe();

            foreach (var onlineUser in onlineUsersButMe)
            {
                var onlineUsersButIt = OnlineUsers.Where(x => x.Name != onlineUser.Name).Select(x => x.Name).ToArray();

                this.SendNotifyToClient(onlineUser.ClientReceiverSocket, "getOnlineUsers", onlineUsersButIt);
            }
        }

        private List<User> OnlineUsersButMe()
        {
            return OnlineUsers.Where(x => x.Name != this.Name).ToList();
        }

        public void Dispose()
        {
            ClientReceiverSocket.Shutdown(SocketShutdown.Both);
            ClientReceiverSocket.Close();

            ClientTransmitterSocket.Shutdown(SocketShutdown.Both);
            ClientTransmitterSocket.Close();
        }
    }
}
