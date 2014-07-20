using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Network
{
    using System.Net.Sockets;
    using Gap.Network.Helper;

    /// <summary>
    /// Represents request and response messages
    /// </summary>
    public class Message
    {
        public string Name { get; set; }

        public string[] Parameters { get; set; }

        public Message()
        {
            Name = "OK";
            Parameters = new string[] { };
        }

        public static Message Receive(Socket socket)
        {
            string[] responsetParts = socket.Receive().Split(';');

            string[] parameters = new string[] { };

            if (responsetParts.Length > 1)
            {
                parameters = responsetParts[1].Split(',');
            }

            return new Message { Name = responsetParts[0], Parameters = parameters };
        }

        /// <summary>
        /// Sends a normal response
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="parameters"></param>
        public static void Send(Socket socket, params string[] parameters)
        {
            Send(socket, "OK", parameters);
        }

        public static void Send(Socket socket, string name, params string[] parameters)
        {
            Send(socket, new Message { Name = name, Parameters = parameters });
        }

        public static void Send(Socket socket, Message requestMessage)
        {
            socket.Send(requestMessage.ToString());
        }

        public override string ToString()
        {
            string parameters = string.Join(",", Parameters).TrimEnd(',');

            return string.Format("{0};{1}", Name, parameters).TrimEnd(';');
        }
    }
}
