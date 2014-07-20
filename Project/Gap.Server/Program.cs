using System;

namespace Gap.Server
{
    using Gap.Network;

    internal class Program
    {
        private static Server server;

        private static void Main(string[] args)
        {
            server = new Server();

            server.ClientConnected += ServerClientConnected;

            server.Start();
        }

        static void ServerClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("Client accepted from {0}", e.TcpClient.Client.RemoteEndPoint);

            using (var user = new User(e.TcpClient.Client))
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
}
