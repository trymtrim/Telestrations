using System;
using System.Collections;
using UnityEngine;

namespace Telestrations.Server
{
    public class Client : MonoBehaviour
    {
        public static Client ClientConnection;

        public Action<string> OnMessageFromServer = delegate { };
        public Action OnConnectedToServer = delegate { };

        private WebSocket _server;

        private string _ipAndPort;

        private void Awake()
        {
            if (ClientConnection == null)
            {
                ClientConnection = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void ConnectToServer(string ipAndPort)
        {
            StartCoroutine(Connect(ipAndPort));
        }

        private IEnumerator Connect(string ipAndPort)
        {
            _ipAndPort = ipAndPort;

            _server = new WebSocket(new Uri($"ws://{ipAndPort}"));

            yield return StartCoroutine(_server.Connect());

            OnConnected(ipAndPort);

            while (true)
            {
                string message = _server.ReceiveString();

                if (message != null)
                {
                    OnMessage(message);
                }

                if (_server.GetError() != null)
                {
                    Debug.LogError("Error: " + _server.GetError());
                    OnDisconnected(ipAndPort);
                    break;
                }

                yield return 0;
            }

            _server.Close();
        }

        private void DisconnectFromServer()
        {
            _server.Close();
            _server = null;
        }

        private void OnConnected(string ipAndPort)
        {
            OnConnectedToServer.Invoke();

            Debug.Log($"Connected to game server [{ipAndPort}]");
        }

        private void OnDisconnected(string ipAndPort)
        {
            Debug.Log($"Disconnected from game server [{ipAndPort}]");
        }

        private void OnMessage(string message)
        {
            OnMessageFromServer.Invoke(message);

            Debug.Log(message);
        }

        private void OnDestroy()
        {
            if (_server == null)
                return;

            _server.Close();
            _server = null;
        }
    }
}
