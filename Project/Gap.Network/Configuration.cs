using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Network
{
    using System.Net;

    public class Configuration
    {
        public static int ServerPort = 23605;

        public static IPAddress ServerAddress = IPAddress.Loopback;
    }
}
