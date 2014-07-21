using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Network
{
    using System.Configuration;
    using System.Net;

    public class Configuration
    {
        public IPAddress ServerAddress { get; set; }

        public int ServerPort { get; set; }

        public static Configuration Load()
        {
            IPAddress serverAddress;

            if (!IPAddress.TryParse(ConfigurationManager.AppSettings["ServerAddress"], out serverAddress))
            {
                serverAddress = IPAddress.Loopback;
            }


            int serverPort;

            if (!int.TryParse(ConfigurationManager.AppSettings["ServerPort"], out serverPort))
            {
                serverPort = 23605;
            }

            return new Configuration { ServerAddress = serverAddress, ServerPort = serverPort };
        }
    }
}
