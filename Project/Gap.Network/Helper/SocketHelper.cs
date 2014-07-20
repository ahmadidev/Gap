using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Network.Helper
{
    using System.Net.Sockets;

    public static class SocketHelper
    {
        public static void Send(this Socket socket, string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }

        public static string Receive(this Socket socket)
        {
            byte[] buffer = new byte[1024];

            int len = socket.Receive(buffer);

            return buffer.AsString(len);
        }
    }
}
