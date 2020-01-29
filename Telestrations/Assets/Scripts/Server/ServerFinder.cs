using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Telestrations.Server
{
    public class ServerFinder : IDisposable
    {
        public Action<string, string> OnGameServerFound;
        public Action<string> OnGameServerRemoved;

        private UdpClient udp = new UdpClient(15000);

        private Dictionary<string, ServerState> _serverStates = new Dictionary<string, ServerState>();

        private bool _isActive = true;

        public ServerFinder()
        {
            StartListening();

            RefreshGameServerList();

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

                GameServerFound(message, message); //TODO: Change to actual name

                Debug.Log(message);
            }

            yield return 0;
        }

        private void GameServerFound(string gameServerName, string gameServerIp)
        {
            if (_serverStates.ContainsKey(gameServerIp))
                _serverStates[gameServerIp].ResetTimer();
            else
                _serverStates.Add (gameServerIp, new ServerState(gameServerIp));

            OnGameServerFound.Invoke(gameServerName, gameServerIp);
        }

        private async void RefreshGameServerList()
        {
            while (_isActive)
            {
                await Task.Delay(500);

                List<ServerState> serversToRemove = new List<ServerState>();

                foreach (KeyValuePair<string, ServerState> serverState in _serverStates)
                {
                    ServerState serverStateToCheck = serverState.Value;

                    if (serverStateToCheck.GetTimeSinceLastUpdate() > 1.5f)
                        serversToRemove.Add(serverStateToCheck);
                }

                foreach (ServerState serverState in serversToRemove)
                {
                    _serverStates.Remove(serverState.IPAddress);
                    OnGameServerRemoved(serverState.IPAddress);
                }
            }
        }

        public virtual void Dispose()
        {
            udp.Dispose();
            udp = null;

            _isActive = false;

            Debug.Log("Stopped finding servers");
        }
    }
}
