using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Telestrations.Server
{
    public class ServerFinder : IDisposable
    {
        public Action<string, string> OnGameServerFound;

        private UdpClient udp = new UdpClient(15000);

        public ServerFinder()
        {
            StartListening();

            Debug.Log("Started finding servers");
        }

        private void StartListening()
        {
            udp.BeginReceive(Receive, new object());
        }

        private void Receive(IAsyncResult ar)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(ReceiveAsync(ar));
        }

        private IEnumerator ReceiveAsync(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 15000);

            if (udp != null)
            {
                byte[] bytes = udp.EndReceive(ar, ref ip);


                string message = Encoding.ASCII.GetString(bytes);

                StartListening();

                OnGameServerFound.Invoke(message, message); //TODO: Change to actual name

                Debug.Log(message);
            }

            yield return 0;
        }

        public virtual void Dispose()
        {
            udp.Dispose();
            udp = null;

            Debug.Log("Stopped finding servers");
        }
    }
}
