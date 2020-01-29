using System.Diagnostics;

namespace Telestrations.Server
{
    public class ServerState
    {
        public string IPAddress;

        private Stopwatch _timer = new Stopwatch();
        
        public ServerState(string serverIp)
        {
            IPAddress = serverIp;

            _timer.Start();
        }

        public void ResetTimer()
        {
            _timer.Restart();
        }

        public float GetTimeSinceLastUpdate()
        {
            return (float) _timer.Elapsed.TotalSeconds;
        }
    }
}
