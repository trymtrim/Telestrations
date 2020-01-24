using System;
using System.Collections;
using UnityEngine;

namespace Telestrations.Server
{
    public class Client : MonoBehaviour
    {
        public static Client ClientConnection;

        public WebSocket server;

        //private string _ip = "192.168.1.116";
        //private string _port = "9000";

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
            server = new WebSocket(new Uri($"ws://{ipAndPort}"));

            yield return StartCoroutine(server.Connect());

            OnConnected(ipAndPort);

            while (true)
            {
                string message = server.ReceiveString();

                if (message != null)
                {
                    OnMessage(message);
                }

                if (server.GetError() != null)
                {
                    Debug.LogError("Error: " + server.GetError());
                    OnDisconnected(ipAndPort);
                    break;
                }

                yield return 0;
            }

            server.Close();
        }

        private void OnConnected(string ipAndPort)
        {
            Debug.Log($"Connected to game server [{ipAndPort}]");
        }

        private void OnDisconnected(string ipAndPort)
        {
            Debug.Log($"Disconnected from game server [{ipAndPort}]");
        }

        private void OnMessage(string message)
        {
            Debug.Log(message);
        }

        private void OnDestroy()
        {
            if (server == null)
                return;

            server.Close();
            server = null;
        }
    }
}
