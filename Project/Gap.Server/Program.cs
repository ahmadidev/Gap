using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Server
{
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal class Program
    {
        private static void Main(string[] args)
        {
            IPAddress serverIpAddress = IPAddress.Any;
            const int ServerPort = 23605;
            IPEndPoint serverIpEndPoint = new IPEndPoint(serverIpAddress, ServerPort);
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(serverIpEndPoint);
            serverSocket.Listen(5);

            Console.WriteLine("Server is running on {0}", serverIpEndPoint);

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();

                Thread clientThread = new Thread(() => HandleClient(clientSocket));

                clientThread.Start();

                Console.WriteLine("Client accepted from {0}", clientSocket.RemoteEndPoint);
            }
        }

        private static void HandleClient(Socket socket)
        {
            using (var user = new User(socket))
            {
                do
                {
                    user.ProcessRequest();
                }
                while (user.Status != User.UserStatus.Loggedout);

                Console.WriteLine("{0} is disconnected.", user.Name);
            }

        }
    }

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

        public Socket Socket { get; set; }

        public UserStatus Status { get; set; }

        public User(Socket socket)
        {
            this.Status = UserStatus.Connected;
            this.Socket = socket;

            OnlineUsers.Add(this);
        }

        public void Login(string name)
        {
            this.Status = UserStatus.Loggedin;
            this.Name = name;

            var user = OnlineUsers.Single(x => Equals(x.Socket, this.Socket));
            user.Name = name;

            Console.WriteLine("{0} is loggedin.", user.Name);
        }

        public void Logout()
        {
            var user = OnlineUsers.Single(x => x.Name == Name);
            OnlineUsers.Remove(user);

            this.Status = UserStatus.Loggedout;
        }

        public string GetOnlineUsers()
        {
            StringBuilder stringBuilder = new StringBuilder();

            var onlineUsers = OnlineUsers.Where(x => x.Name != this.Name).ToList();

            if (!onlineUsers.Any())
                return "empty";

            foreach (var onlineUser in onlineUsers)
            {
                stringBuilder.Append(onlineUser.Name + ';');
            }

            return stringBuilder.ToString().TrimEnd(';');
        }

        public void ProcessRequest()
        {
            byte[] buffer = new byte[1024];

            int messageLength = this.Socket.Receive(buffer, SocketFlags.None);

            string[] requestParts = Encoding.ASCII.GetString(buffer, 0, messageLength).Split(';');

            string
                requestName = requestParts[0],
                requestParameter = requestParts[1];

            string response = "OK";

            switch (requestName)
            {
                case "login":
                    this.Login(requestParameter);
                    break;
                case "getOnlineUsers":
                    response = this.GetOnlineUsers();
                    break;
                case "logout":
                    this.Logout();
                    break;
            }


            buffer = Encoding.ASCII.GetBytes(response);

            Socket.Send(buffer);
        }

        public void Dispose()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }
    }
}
