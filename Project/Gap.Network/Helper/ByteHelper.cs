using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Network.Helper
{
    public static class ByteHelper
    {
        public static string AsString(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        public static string AsString(this byte[] buffer, int legth)
        {
            return Encoding.UTF8.GetString(buffer, 0, legth);
        }
    }
}
