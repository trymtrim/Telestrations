using System.Net;

namespace Telestrations.Server
{
    public class ServerHelper
    {
        public static bool IsHost = false;

        public static string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }

            return "127.0.0.1";
        }
    }
}
